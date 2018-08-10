/*
' Copyright (c) 2018  Josue Araúz
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Entities.Modules;
using System;

namespace Jar.Dnn.LaboratoryModule
{
    public class LaboratoryModuleModuleBase : PortalModuleBase
    {
        DotNetNuke.Services.Log.EventLog.EventLogController EventLog = new DotNetNuke.Services.Log.EventLog.EventLogController();
        public Telerik.Web.UI.RadAjaxManager AjaxMan { get; set; }

        public void SendMail(string Name, string Message, string Gender, string Email, string Copy = "")
        {
            int i = 0;
            string strFrom = "";
            string host = "";
            string subject = "";
            string strCc = "";
            string strMessage = GetEmailTemplate();
            string DisplayName = "";

            if (Settings.Contains("Bcc"))
                strCc = Settings["Bcc"].ToString();

            if (Settings.Contains("From"))
                strFrom = Settings["From"].ToString();

            if (Settings.Contains("Host"))
                host = Settings["Host"].ToString();

            if (Settings.Contains("Subject"))
                subject = Settings["Subject"].ToString();

            if (Settings.Contains("DisplayName"))
                DisplayName = Settings["DisplayName"].ToString();
            else
                DisplayName = PortalSettings.PortalName;

            System.Data.DataTable dt = new System.Data.DataTable();

            if (string.IsNullOrEmpty(strFrom) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(host) || string.IsNullOrEmpty(subject))
            {
                throw new Exception("Error al enviar el correo, el módulo no se encuentra configurado correctamente.");
            }

            string appPath = string.Format("{0}Resources/Images/", ControlPath);
            if (appPath.EndsWith("/") == false)
            {
                appPath = appPath + Convert.ToString("/");
            }

            strMessage = strMessage.Replace("$TURNO", GetTimeTitle());
            strMessage = strMessage.Replace("$GENDER", Gender.ToUpper() == "MASCULINO" ? "estimado" : "estimada");
            strMessage = strMessage.Replace("$NOMBRE", Name);
            strMessage = strMessage.Replace("$MENSAJE", Message);

            string logo = appPath + Convert.ToString("LogoMail.png");
            string appName = appPath + Convert.ToString("AppName.png");

            try
            {
                // Instantiate a new instance of MailMessage
                System.Net.Mail.MailMessage mMailMessage = new System.Net.Mail.MailMessage()
                {
                    //Set the sender address of the mail message
                    From = new System.Net.Mail.MailAddress(strFrom, DisplayName),
                };

                System.Net.Mail.AlternateView av1 = System.Net.Mail.AlternateView.CreateAlternateViewFromString(strMessage, null, System.Net.Mime.MediaTypeNames.Text.Html);

                System.Net.Mail.LinkedResource linkedResource = new System.Net.Mail.LinkedResource(Server.MapPath(logo));
                System.Net.Mail.LinkedResource appNameResource = new System.Net.Mail.LinkedResource(Server.MapPath(appName));

                linkedResource.ContentId = "logo";
                linkedResource.ContentType.Name = logo;

                appNameResource.ContentId = "appname";
                appNameResource.ContentType.Name = appName;

                av1.LinkedResources.Add(linkedResource);
                av1.LinkedResources.Add(appNameResource);
                mMailMessage.AlternateViews.Add(av1);


                //Set the recepient address of the mail message
                string[] arrTo = Email.Split(',');

                for (i = arrTo.GetLowerBound(0); i <= arrTo.GetUpperBound(0); i++)
                {
                    if (IsMail(arrTo[i].Trim()))
                    {
                        mMailMessage.To.Add(new System.Net.Mail.MailAddress(arrTo[i].Trim()));
                    }
                }

                if (mMailMessage.To.Count == 0)
                {
                    mMailMessage.To.Add(new System.Net.Mail.MailAddress(strFrom));
                }

                //Check if the cc value is nothing or an empty value
                if (!string.IsNullOrEmpty(Copy))
                {
                    //Set the CC address of the mail message
                    mMailMessage.CC.Add(new System.Net.Mail.MailAddress(Copy));
                }

                //Check if the cc value is nothing or an empty value
                if (!string.IsNullOrEmpty(strCc))
                {
                    //Set the BCC address of the mail message
                    mMailMessage.Bcc.Add(new System.Net.Mail.MailAddress(strCc));
                }

                //Set the subject of the mail message
                mMailMessage.Subject = subject;

                //Set the body of the mail message
                mMailMessage.Body = strMessage;

                //Set the format of the mail message body as HTML
                mMailMessage.IsBodyHtml = true;

                //Instantiate a new instance of SmtpClient
                System.Net.Mail.SmtpClient emailServer = new System.Net.Mail.SmtpClient(host);

                //Send the mail message
                //emailServer.Send(mMailMessage);

                mMailMessage.Dispose();
            }
            catch (System.Net.Mail.SmtpFailedRecipientsException ehttp)
            {
                string err = "";
                for (i = 0; i <= ehttp.InnerExceptions.Length; i++)
                {
                    err = (err + Convert.ToString("SMTP.Send: Error! ")) + ehttp.InnerExceptions[i].StatusCode.ToString() + " Failed to deliver message to " + ehttp.InnerExceptions[i].FailedRecipient.ToString();
                }

                LogError(err);
                throw new Exception(err);
            }
        }

        public bool IsMail(string pEmail)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^(([^<;>;()[\\]\\\\.,;:\\s@\\\"]+" + "(\\.[^<;>;()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@" + "((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}" + "\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+" + "[a-zA-Z]{2,}))$");
            return (reg.IsMatch(pEmail));
        }

        private string GetEmailTemplate()
        {
            string strTemplate = "";
            string filePath = "";
            try
            {
                filePath = Server.MapPath(string.Format("{0}/Resources/Templates/EmailTemplate.html", ControlPath));

                System.IO.StreamReader sr = new System.IO.StreamReader(filePath);
                string line = null;

                do
                {
                    line = sr.ReadLine();
                    strTemplate = strTemplate + line;
                } while (!(line == null));
                sr.Close();
            }
            catch (Exception E)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(E.Message);
            }

            return strTemplate;

        }

        protected string GetTimeTitle()
        {
            string timeTitle = null;
            if (DateTime.Now.Hour < 12)
            {
                timeTitle = "Buenos días";
            }
            else if (DateTime.Now.Hour < 17)
            {
                timeTitle = "Buenas tardes";
            }
            else
            {
                timeTitle = "Buenas noches";
            }
            return timeTitle;
        }

        public void LogError(string Message)
        {
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
            string MethodName = string.Format("{0}-{1}:", stackTrace.GetFrame(1).GetMethod().ReflectedType.ToString(), stackTrace.GetFrame(1).GetMethod().Name);
            EventLog.AddLog(MethodName, Message, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.ADMIN_ALERT);
        }

        public void DisplayAlert(string Message, string Title = "Error", string sType = "Error")
        {
            int width = 300;
            int height = 100;
            int factor = 0;

            if (Message.Length > 115)
            {
                factor = (int)Math.Round(((float)Message.Length - (float)100) / (float)100, 0);
                height += (factor * 70);
            }

            AjaxMan.ResponseScripts.Add(string.Format("showDialogNotification('{0}', '{1}', {2}, {3}, '{4}')", System.Web.HttpUtility.HtmlEncode(Message), System.Web.HttpUtility.HtmlEncode(Title), width, height, sType));
        }
        public void DisplayAlert(Exception ex)
        {
            if (ex.InnerException != null)
            {
                LogError(ex.InnerException.Message);
                DisplayAlert(ex.InnerException.Message);
            }
            else
            {
                LogError(ex.Message);
                DisplayAlert(ex.Message);
            }
        }


        public System.Data.DataTable ConstructTable(string typeTable)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            switch (typeTable.ToUpper())
            {
                case "USERS":
                    dt.Columns.Add("USERID");
                    dt.Columns.Add("USERNAME");
                    dt.Columns.Add("DISPLAYNAME");
                    dt.Columns.Add("NAME");
                    dt.Columns.Add("LASTNAME");
                    dt.Columns.Add("EMAIL");
                    dt.Columns.Add("AUTHORIZED");
                    dt.Columns.Add("LOCKED_OUT");
                    dt.Columns.Add("SCHEDULE_DES");
                    dt.Columns.Add("SCHEDULE_ID"); ;
                    dt.Columns.Add("IS_ONLINE"); ;
                    dt.Columns.Add("PASSWORD");
                    break;
                case "IP_ADDRESS":
                    dt.Columns.Add("IP_ADDRESS");
                    break;
                case "SCHEDULES":
                    dt.Columns.Add("SCH_ID");
                    dt.Columns.Add("DAY_ID");
                    dt.Columns.Add("DAY");
                    dt.Columns.Add("TIME_START");
                    dt.Columns.Add("TIME_END");
                    break;
            }

            return dt;
        }

        public System.Data.DataTable GetDataTable(Object obj)
        {
            Telerik.Web.UI.RadGrid grd = (Telerik.Web.UI.RadGrid)obj;
            System.Data.DataTable dtRecords = new System.Data.DataTable();

            foreach (Telerik.Web.UI.GridColumn col in grd.Columns)
            {
                if (col.ColumnType == "GridBoundColumn" || col.ColumnType == "GridDateTimeColumn")
                {
                    System.Data.DataColumn colString = new System.Data.DataColumn(col.UniqueName);
                    dtRecords.Columns.Add(colString);
                }

            }

            foreach (Telerik.Web.UI.GridDataItem row in grd.Items)
            {
                System.Data.DataRow dr = dtRecords.NewRow();
                foreach (Telerik.Web.UI.GridColumn col in grd.Columns)
                {
                    if (col.ColumnType == "GridBoundColumn" || col.ColumnType == "GridDateTimeColumn")
                    {
                        dr[col.UniqueName] = row[col.UniqueName].Text;
                    }
                }
                dtRecords.Rows.Add(dr);
            }

            return dtRecords;

        }
    }

    public class DnnUsers
    {
        private string[] roles = { "Registered Users", "Subscribers" };

        public void UpdateDnnUser(DotNetNuke.Entities.Users.UserInfo user)
        {
            DotNetNuke.Security.Membership.MembershipProvider objMembershipProvider = DotNetNuke.Security.Membership.MembershipProvider.Instance();

            objMembershipProvider.UpdateUser(user);

        }

        public bool CreateDnnUser(DotNetNuke.Entities.Users.UserInfo user, string roleName = "")
        {
            DotNetNuke.Security.Membership.UserCreateStatus objUserCreateStatus = DotNetNuke.Security.Membership.UserCreateStatus.AddUser;
            DotNetNuke.Security.Membership.MembershipProvider objMembershipProvider = DotNetNuke.Security.Membership.MembershipProvider.Instance();

            objUserCreateStatus = objMembershipProvider.CreateUser(ref user);

            bool status = false;

            switch (objUserCreateStatus)
            {
                case DotNetNuke.Security.Membership.UserCreateStatus.Success:
                    foreach (string rol in roles)
                    {
                        AddUserToRole(user, rol);
                    }

                    if (!string.IsNullOrEmpty(roleName))
                    {
                        AddUserToRole(user, roleName);
                    }
                    status = true;
                    break;
                case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateEmail:
                    throw new Exception("El correo ingresado ya se encuentra registrado en el sistema");
                case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateUserName:
                    throw new Exception("El usuario ingresado ya se encuentra registrado en el sistema");
                case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateProviderUserKey:
                    throw new Exception("El key ingresado ya se encuentra registrado en el sistema");
                case DotNetNuke.Security.Membership.UserCreateStatus.InvalidEmail:
                    throw new Exception("El correo ingresado no es válido");
                case DotNetNuke.Security.Membership.UserCreateStatus.InvalidPassword:
                    throw new Exception("La contraseña ingresada no es válida");
                case DotNetNuke.Security.Membership.UserCreateStatus.InvalidUserName:
                    throw new Exception("El usuario ingresado no es válido");
                case DotNetNuke.Security.Membership.UserCreateStatus.UserRejected:
                    throw new Exception("El usuario ingresado fue rechazado por el sistema");
                default:
                    throw new Exception(objUserCreateStatus.ToString());
            }

            return status;

        }

        public bool AddUserToRole(DotNetNuke.Entities.Users.UserInfo user, string roleName)
        {
            bool rc = false;
            DotNetNuke.Security.Roles.RoleController roleCtl = new DotNetNuke.Security.Roles.RoleController();
            DotNetNuke.Security.Roles.RoleInfo newRole = roleCtl.GetRoleByName(user.PortalID, roleName);

            if (newRole == null)
            {
                throw new Exception("No existe el Rol [" + roleName + "] en el sistema");
            }
            else if (user == null)
            {
                throw new Exception("La información de usuario se encuentra vacía");
            }
            else
            {
                rc = user.IsInRole(roleName);
                if (!rc)
                {
                    roleCtl.AddUserRole(user.PortalID, user.UserID, newRole.RoleID, DateTime.MinValue, DateTime.MaxValue);
                    user = DotNetNuke.Entities.Users.UserController.GetUserById(user.PortalID, user.UserID);
                    rc = user.IsInRole(roleName);
                }
            }

            return rc;
        }
    }
}