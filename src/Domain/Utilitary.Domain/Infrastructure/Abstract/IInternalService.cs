namespace Ibero.Services.Utilitary.Domain.Infrastructure.Abstract
{
    using Ibero.Services.Utilitary.Domain.Exceptions;
    using Ibero.Services.Utilitary.Domain.Infrastructure.Configuration;
    using Microsoft.Extensions.Configuration;
    using Newtonsoft.Json;
    using SelectPdf;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO; 

    public class IInternalService
    {
        private readonly string _connection;

        public IInternalService(IConfiguration configuration)
        {
            _connection = configuration.GetConnectionString("UtilitaryDb");
        }

        public string getHomologationID(string TableReference, string IdReference, string HomReference, string CodeReference)
        {
            var response = "";
            try
            { 
                using (SqlConnection sql = new SqlConnection(_connection))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT " + HomReference + " FROM " + TableReference + " WHERE " + IdReference + " = " + CodeReference + ";", sql))
                    {
                        cmd.CommandType = CommandType.Text;
                        sql.Open();

                        using (var sqlReader = cmd.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                response += sqlReader[0].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.InsErrorLog(this.GetType().FullName, "SELECT " + HomReference + " FROM " + TableReference + " WHERE " + IdReference + " = " + CodeReference + ";", ex.StackTrace, ex.Message);
                response = CodeReference.ToString();
            }
            return response;
        }

        public string getTemplateBody(string CodeTemplate, string ReplaceValues)
        {
            var htmlBody = "";

            try
            {
                using (SqlConnection sql = new SqlConnection(_connection))
                {
                    using (SqlCommand cmd = new SqlCommand("IBEP_SP_Templates", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Reference", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@CodTemplate", SqlDbType.VarChar).Value = CodeTemplate;

                        sql.Open();
                        TemplatesBody objectBody = new TemplatesBody();

                        using (var sqlReader = cmd.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                objectBody.IdTemplate = Convert.ToInt32(sqlReader[0]);
                                objectBody.CodTemplate = sqlReader[1].ToString();
                                objectBody.TypeTemplate = sqlReader[2].ToString();
                                objectBody.NameTemplate = sqlReader[3].ToString();
                                objectBody.BodyTemplate = sqlReader[4].ToString();
                                objectBody.StaStatus = Convert.ToBoolean(sqlReader[5]);
                            }
                        }

                        htmlBody = objectBody.BodyTemplate.ToString();


                        var listReference = JsonConvert.DeserializeObject<Dictionary<string, string>>(ReplaceValues.ToString());
                        foreach (var datList in listReference)
                        {
                            if (datList.Value != null)
                            {
                                htmlBody = htmlBody.Replace('{' + datList.Key + '}', datList.Value.Replace("\n", "<br>"));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.InsErrorLog(this.GetType().FullName, CodeTemplate, ex.StackTrace, ex.Message);
                htmlBody = ex.Message.ToString();
            }
            return htmlBody;
        }

        public string getTemplateName(string CodeTemplate, string ReplaceValues)
        {
            var NameTemplate = "";

            try
            {
                using (SqlConnection sql = new SqlConnection(_connection))
                {
                    using (SqlCommand cmd = new SqlCommand("IBEP_SP_Templates", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Reference", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@CodTemplate", SqlDbType.VarChar).Value = CodeTemplate;

                        sql.Open();
                        TemplatesBody objectBody = new TemplatesBody();

                        using (var sqlReader = cmd.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                objectBody.NameTemplate = sqlReader[3].ToString();
                            }
                        }

                        NameTemplate = objectBody.NameTemplate.ToString();

                        if (ReplaceValues != null)
                        {
                            var listReference = JsonConvert.DeserializeObject<Dictionary<string, string>>(ReplaceValues.ToString());
                            foreach (var datList in listReference)
                            {
                                NameTemplate = NameTemplate.Replace('{' + datList.Key + '}', datList.Value);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.InsErrorLog(this.GetType().FullName, CodeTemplate, ex.StackTrace, ex.Message);
                NameTemplate = ex.Message.ToString();
            }

            return NameTemplate;
        }

        public List<ConfigModule> getConfigModule(string codeModule)
        {
            string Data = "";
            List<ConfigModule> confModule;

            using (SqlConnection sql = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("IBEP_SP_Configuration", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@Reference", SqlDbType.Int).Value = 3;
                    cmd.Parameters.Add("@CodModule", SqlDbType.VarChar).Value = codeModule;

                    sql.Open();

                    using (var sqlReader = cmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            Data += sqlReader[0].ToString();
                        }
                    }

                    confModule = JsonConvert.DeserializeObject<List<ConfigModule>>(Data);
                }
            }

            return confModule;
        }

        public object GeneratePDFFile(string codeFile, string htmlBody)
        {
            string baseUrl = "";
            string Data = "";
            object resultFile;

            using (SqlConnection sql = new SqlConnection(_connection))
            {
                using (SqlCommand cmd = new SqlCommand("IBEP_SP_Files", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var jsonValue = JsonConvert.SerializeObject(new { Nam_Module = codeFile.ToString() }).ToString();
                    cmd.Parameters.Add("@Reference", SqlDbType.VarChar).Value = "GETMODULE";
                    cmd.Parameters.Add("@ListData", SqlDbType.VarChar).Value = jsonValue;

                    sql.Open();

                    using (var sqlReader = cmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            Data += sqlReader[0].ToString();
                        }
                    }
                }

            

                // instantiate a html to pdf converter object
                HtmlToPdf converter = new HtmlToPdf();

                // Setting Pdf Page Properties
                converter.Options.PdfPageSize = PdfPageSize.Letter;
                converter.Options.MarginBottom = 20;
                converter.Options.MarginLeft = 15;
                converter.Options.MarginRight = 15;
                converter.Options.MarginTop = 20;
                converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.NoAdjustment;
                converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.NoAdjustment;

                // create a new pdf document converting the html code
                PdfDocument doc = converter.ConvertHtmlString(htmlBody, baseUrl);

                // save pdf document
                var newName = DateTime.Now.Ticks + ".pdf";
            
                doc.Close();

             

                using (SqlCommand cmd = new SqlCommand("IBEP_SP_Files", sql))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //var jsonValue = JsonConvert.SerializeObject(fileAdd).ToString();
                    cmd.Parameters.Add("@Reference", SqlDbType.VarChar).Value = "ADDFILE";
                    //cmd.Parameters.Add("@ListData", SqlDbType.VarChar).Value = jsonValue;
                    cmd.Parameters.Add("@DataResum", SqlDbType.Int).Value = 1;
                    Data = "";

                    using (var sqlReader = cmd.ExecuteReader())
                    {
                        while (sqlReader.Read())
                        {
                            Data += sqlReader[0].ToString();
                        }
                    }

                    resultFile = Data;

                    sql.Close();
                }
            }

            return resultFile;

        }

        public int AddMailOutPut(object mailData, object DataFile)
        {
            int idMailFeedBack = 0;
            try
            {
                using (SqlConnection sql = new SqlConnection(_connection))
                {
                    using (SqlCommand cmd = new SqlCommand("IBEP_SP_Mailing", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Reference", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@JsonData", SqlDbType.VarChar).Value = mailData;
                        cmd.Parameters.Add("@JsonAttach", SqlDbType.VarChar).Value = DataFile;

                        sql.Open();

                        using (var sqlReader = cmd.ExecuteReader())
                        {
                            while (sqlReader.Read())
                            {
                                idMailFeedBack = Convert.ToInt32(sqlReader[0].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DeleteFailureException(nameof(IInternalService), ex.Message, ex.Message);
            }
            return idMailFeedBack;
        }

        public void InsErrorLog(string Method, string Request, string Response, string Message)
        {
            try
            {
                /*
                string IP = System.Net.Dns.GetHostName() + " - " + System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName())[2].ToString();
                */
                using (SqlConnection sql = new SqlConnection(_connection))
                {

                    using (SqlCommand cmd = new SqlCommand("IBEP_InsErrorLog", sql))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Method", SqlDbType.VarChar).Value = Method;
                        cmd.Parameters.Add("@Request", SqlDbType.VarChar).Value = Request;
                        cmd.Parameters.Add("@Response", SqlDbType.VarChar).Value = Response;
                        cmd.Parameters.Add("@Message", SqlDbType.VarChar).Value = Message;
                        cmd.Parameters.Add("@IP", SqlDbType.VarChar).Value = "NA";//IP;

                        sql.Open();
                        cmd.ExecuteNonQuery();
                    }

                }

            }
            catch (Exception ex)
            {
                throw new DeleteFailureException(nameof(IInternalService), ex.Message, ex.Message);
            }
            return;
        }
    }
}