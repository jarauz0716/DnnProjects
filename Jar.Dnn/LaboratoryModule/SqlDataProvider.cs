using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Jar.Dnn.LaboratoryModule.Data
{
    /// <summary>
    /// Summary description for SqlDataProvider
    /// </summary>
    public class SqlDataProvider : DataProvider
    {
        #region Private Members

        private const string ProviderType = "data";
        private const string ModuleQualifier = "LaboratoryModule";
        private const int CmdTimeOut = 180;

        private readonly ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(ProviderType);
        private readonly string _connectionString;
        private readonly string _providerPath;
        private readonly string _objectQualifier;
        private readonly string _databaseOwner;

        #endregion

        #region Constructors

        public SqlDataProvider()
        {

            // Read the configuration specific information for this provider
            Provider objProvider = (Provider)(_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);

            // Read the attributes for this provider

            //Get Connection string from web.config
            _connectionString = Config.GetConnectionString();

            if (string.IsNullOrEmpty(_connectionString))
            {
                // Use connection string specified in provider
                _connectionString = objProvider.Attributes["connectionString"];
            }

            _providerPath = objProvider.Attributes["providerPath"];

            _objectQualifier = objProvider.Attributes["objectQualifier"];
            if (!string.IsNullOrEmpty(_objectQualifier) && _objectQualifier.EndsWith("_", StringComparison.Ordinal) == false)
            {
                _objectQualifier += "_";
            }

            _databaseOwner = objProvider.Attributes["databaseOwner"];
            if (!string.IsNullOrEmpty(_databaseOwner) && _databaseOwner.EndsWith(".", StringComparison.Ordinal) == false)
            {
                _databaseOwner += ".";
            }

        }

        #endregion

        #region Properties

        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
        }

        public string ProviderPath
        {
            get
            {
                return _providerPath;
            }
        }

        public string ObjectQualifier
        {
            get
            {
                return _objectQualifier;
            }
        }

        public string DatabaseOwner
        {
            get
            {
                return _databaseOwner;
            }
        }

        // used to prefect your database objects (stored procedures, tables, views, etc)
        private string NamePrefix
        {
            get { return DatabaseOwner + ObjectQualifier + ModuleQualifier; }
        }

        #endregion

        #region Private Methods

        private static object GetNull(object field)
        {
            return Null.GetNull(field, DBNull.Value);
        }

        private System.Data.DataTable ExecuteQuery(string command, System.Data.CommandType cmdType, System.Data.SqlClient.SqlParameter[] parameters = null)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);

            try
            {
                sqlcnn.Open();

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = cmdType;

                    if (parameters != null)
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return dt;
        }

        #endregion

        #region Public Methods

        public override DataTable GetDoctors(int Id = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "VM_SP_Get_Doctors";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                dt = ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetDoctors, " + ex.Message, ex);
            }

            return dt;
        }
        public override DataTable GetPatients(int Id = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "VM_SP_Get_Patients";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                dt = ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetPatients, " + ex.Message, ex);
            }

            return dt;
        }
        public override DataTable GetPatientLabs(int PatientId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "VM_SP_Get_Laboratories";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@PatientId", PatientId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                dt = ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetPatientLabs, " + ex.Message, ex);
            }

            return dt;
        }
        public override DataTable GetLabDetails(int LabId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "VM_SP_Get_Laboratory_Details";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@LaboratoryId", LabId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                dt = ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetLabDetails, " + ex.Message, ex);
            }

            return dt;
        }

        public override void SetPatient(string PersonalId, string Gender, DateTime Birthday, string FirstName, string SecondName, string LastName, string SecondLastName, string Phone, string CellPhone, string Whatsapp, string Email, string Address, int Id = 0)
        {
            string command = "VM_SP_Set_Patient";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[13];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@PersonalId", PersonalId)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 21
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@FirstName", FirstName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[3] = new System.Data.SqlClient.SqlParameter("@SecondName", SecondName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[4] = new System.Data.SqlClient.SqlParameter("@LastName", LastName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[5] = new System.Data.SqlClient.SqlParameter("@SecondLastName", SecondLastName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[6] = new System.Data.SqlClient.SqlParameter("@Gender", Gender)
                {
                    SqlDbType = System.Data.SqlDbType.Char,
                    Size = 1
                };
                parameters[7] = new System.Data.SqlClient.SqlParameter("@Birthday", Birthday)
                {
                    SqlDbType = System.Data.SqlDbType.DateTime,
                    Direction = ParameterDirection.Input
                };
                parameters[8] = new System.Data.SqlClient.SqlParameter("@Address", Address)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 250
                };
                parameters[9] = new System.Data.SqlClient.SqlParameter("@Phone", Phone)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 10
                };
                parameters[10] = new System.Data.SqlClient.SqlParameter("@CellPhone", CellPhone)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 10
                };
                parameters[11] = new System.Data.SqlClient.SqlParameter("@Whatsapp", Whatsapp)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 10
                };
                parameters[12] = new System.Data.SqlClient.SqlParameter("@Email", Email)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };

                ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetPatient, " + ex.Message, ex);
            }
        }
        public override int SetLaboratory(int PatientId, int DoctorId, string LabName, string Observation, int UserId, int Id = 0)
        {
            string command = "VM_SP_Set_Laboratory";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[6];
            DataTable dt = new DataTable();

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@PatientId", PatientId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@DoctorId", DoctorId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                parameters[3] = new System.Data.SqlClient.SqlParameter("@LabName", LabName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[4] = new System.Data.SqlClient.SqlParameter("@Observation", Observation)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 250
                };
                parameters[5] = new System.Data.SqlClient.SqlParameter("@UserId", UserId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                dt = ExecuteQuery(command, CommandType.StoredProcedure, parameters);

                if (dt.Rows.Count > 0)
                    Id = int.Parse(dt.Rows[0]["Id"].ToString());

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetLaboratory, " + ex.Message, ex);
            }

            return Id;
        }
        public override void SetLaboratoryDetail(int LaboratoryId, string LabDetail, string LabResult, string LabReference, string LabUnit, bool Remark, int UserId, int Id = 0)
        {
            string command = "VM_SP_Set_Laboratory_Detail";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[8];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@LaboratoryId", LaboratoryId)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 21
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@LabDetail", LabDetail)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[3] = new System.Data.SqlClient.SqlParameter("@LabResult", LabResult)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 50
                };
                parameters[4] = new System.Data.SqlClient.SqlParameter("@LabReference", LabReference)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 15
                };
                parameters[5] = new System.Data.SqlClient.SqlParameter("@LabUnit", LabUnit)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 10
                };
                parameters[6] = new System.Data.SqlClient.SqlParameter("@Remark", Remark)
                {
                    SqlDbType = System.Data.SqlDbType.Bit,
                    Direction = ParameterDirection.Input
                };
                parameters[7] = new System.Data.SqlClient.SqlParameter("@UserId", UserId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetLaboratoryDetail, " + ex.Message, ex);
            }
        }
        public override void SetClosedLaboratory(int Id, int UserId)
        {
            string command = "VM_SP_Set_Closed_Laboratory";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[2];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@UserId", UserId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetLabDetails, " + ex.Message, ex);
            }
        }
        public override void SetObservation(int Id, string Observation, int UserId)
        {
            SetLaboratory(0, 0, string.Empty, Observation, UserId, Id);
        }
        public override void DeleteLaboratory(int Id)
        {
            string command = "VM_SP_Delete_Laboratory";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.DeleteLaboratory, " + ex.Message, ex);
            }
        }

        public override void DeleteLaboratoryDetail(int Id)
        {
            string command = "VM_SP_Delete_Laboratory_Detail";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@Id", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Input
                };

                ExecuteQuery(command, CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.DeleteLaboratoryDetail, " + ex.Message, ex);
            }
        }

        #region Users
        public override int CheckUserInSchedule(int user_id, string dayOfweek, TimeSpan currentTime)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_CHECK_SCHEDULE";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[3];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@USER_ID", user_id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@DAY_WEEK", dayOfweek)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 10
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@CURRENT_TIME", currentTime)
                {
                    SqlDbType = System.Data.SqlDbType.Time,
                    Direction = System.Data.ParameterDirection.Input
                };

                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.CheckUserInSchedule, " + ex.Message, ex);
            }

            return 1;
        }

        public override int GetBadLogonCount(string userName, string Ip)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "DNNCL_GET_BADLOGON_COUNT";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[2];
            int badLogonCount = -1;

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@USERNAME", userName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 100
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@IP", Ip)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 15
                };
                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure, parameters);

                foreach (System.Data.DataRow drow in dt.Rows)
                    badLogonCount = int.Parse(drow["BADLOGON_COUNT"].ToString());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetBadLogonCount, " + ex.Message, ex);
            }

            return badLogonCount;
        }

        public override void SetLoginLog(string ip, string userName, string status)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "DNNCL_SET_LOGIN_LOG";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[3];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@IP", ip)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 15
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@USERNAME", userName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 100
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@STATUS", status)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 25
                };

                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetLoginLog, " + ex.Message, ex);
            }
        }

        public override bool CheckIpBanned(string ip)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "DNNCL_CHECK_BANNED_IP";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];
            bool banned = false;

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@IP", ip)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 15
                };

                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure, parameters);

                foreach (System.Data.DataRow drow in dt.Rows)
                    banned = drow["BANNED"].ToString() == "1" ? true : false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.CheckIpBanned, " + ex.Message, ex);
            }

            return banned;
        }

        public override System.Data.DataTable GetSchedules(int scheduleId = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_GET_SCHEDULES";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@SCHEDULE_ID", scheduleId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetSchedules, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return dt;
        }

        public override System.Data.DataTable GetUsersSchedules(int user_id = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_GET_SCHEDULE_USER";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);

            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@USER_ID", user_id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetUsersSchedules, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return dt;
        }

        public override System.Data.DataTable GetSchedulesDays()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_GET_SCHEDULES_DAYS";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);

            System.Data.SqlClient.SqlParameter[] parameters = null;

            try
            {
                sqlcnn.Open();

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetSchedulesDays, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return dt;
        }

        public override int SetSchedule(string description, int id = 0)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_SET_SCHEDULE";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[2];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@ID", id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@DESCRIPTION", description)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 250
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    return int.Parse(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetSchedule, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return 0;
        }

        public override int SetScheduleLimits(int scheduleId, int dayId, TimeSpan timeStart, TimeSpan timeEnd)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_SET_SCHEDULE_LIMIT";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[4];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@SCHEDULE_ID", scheduleId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@DAY_ID", dayId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@TIME_START", timeStart)
                {
                    SqlDbType = System.Data.SqlDbType.Time,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[3] = new System.Data.SqlClient.SqlParameter("@TIME_END", timeEnd)
                {
                    SqlDbType = System.Data.SqlDbType.Time,
                    Direction = System.Data.ParameterDirection.Input
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetScheduleLimits, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return 1;
        }

        public override int DeleteScheduleLimits(int scheduleId, int dayId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_DELETE_SCHEDULE_LIMIT";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[2];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@SCHEDULE_ID", scheduleId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@DAY_ID", dayId)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.DeleteScheduleLimits, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return 1;
        }

        public override int SetUserSchedule(int user_id, int sch_id)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_SET_SCHEDULE_USER";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[2];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@USER_ID", user_id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@SCHEDULE_ID", sch_id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetUserSchedule, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return 1;
        }

        public override int DeleteSchedule(int Id)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "CDM_DELETE_SCHEDULE";
            System.Data.SqlClient.SqlConnection sqlcnn = new System.Data.SqlClient.SqlConnection(ConnectionString);
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                sqlcnn.Open();

                parameters[0] = new System.Data.SqlClient.SqlParameter("@ID", Id)
                {
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = System.Data.ParameterDirection.Input
                };

                using (System.Data.SqlClient.SqlCommand sqlcmd = new System.Data.SqlClient.SqlCommand(command, sqlcnn))
                {
                    sqlcmd.CommandTimeout = CmdTimeOut;
                    sqlcmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if ((parameters != null))
                    {
                        sqlcmd.Parameters.AddRange(parameters);
                    }

                    using (System.Data.SqlClient.SqlDataAdapter sqlda = new System.Data.SqlClient.SqlDataAdapter(sqlcmd))
                    {
                        sqlda.Fill(dt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.DeleteSchedule, " + ex.Message, ex);
            }
            finally
            {
                sqlcnn.Close();
            }

            return 1;
        }

        public override System.Data.DataTable GetBannedIP()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "DNNCL_GET_BANNED_IP";

            try
            {
                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.GetBannedIP, " + ex.Message, ex);
            }

            return dt;
        }

        public override int SetBannedIP(string Ip, string userName, string description)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "DNNCL_SET_BANNED_IP";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[3];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@IP", Ip)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 15
                };
                parameters[1] = new System.Data.SqlClient.SqlParameter("@USERNAME", userName)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 100
                };
                parameters[2] = new System.Data.SqlClient.SqlParameter("@DESCRIPTION", description)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 250
                };

                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.SetBannedIP, " + ex.Message, ex);
            }

            return 1;
        }

        public override int DeleteBannedIP(string Ip)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            string command = "DNNCL_DELETE_BANNED_IP";
            System.Data.SqlClient.SqlParameter[] parameters = new System.Data.SqlClient.SqlParameter[1];

            try
            {
                parameters[0] = new System.Data.SqlClient.SqlParameter("@IP", Ip)
                {
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 15
                };

                dt = ExecuteQuery(command, System.Data.CommandType.StoredProcedure, parameters);

            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la función SqlDataProvider.DeleteBannedIP, " + ex.Message, ex);
            }

            return 1;
        }

        #endregion
        #endregion
    }
}