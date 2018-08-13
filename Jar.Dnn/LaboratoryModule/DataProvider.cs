using DotNetNuke.Common.Utilities;
using DotNetNuke.Framework.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Jar.Dnn.LaboratoryModule.Data
{
    /// <summary>
    /// Summary description for DataProvider
    /// </summary>
    public abstract class DataProvider
    {

        #region Shared/Static Methods

        private static DataProvider provider;

        // return the provider
        public static DataProvider Instance()
        {
            if (provider == null)
            {
                //Type.GetType("TypeName,DllName");
                //const string assembly = "APC.Dnn.VacationsControl.Data.SqlDataprovider,APC.Dnn.VacationsControl";
                //Type objectType = Type.GetType(assembly, true, true);
                Type objectType = typeof(Data.SqlDataProvider);

                provider = (DataProvider)Activator.CreateInstance(objectType);
                DataCache.SetCache(objectType.FullName, provider);
            }

            return provider;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Not returning class state information")]
        public static IDbConnection GetConnection()
        {
            const string providerType = "data";
            ProviderConfiguration _providerConfiguration = ProviderConfiguration.GetProviderConfiguration(providerType);

            Provider objProvider = ((Provider)_providerConfiguration.Providers[_providerConfiguration.DefaultProvider]);
            string _connectionString;
            if (!String.IsNullOrEmpty(objProvider.Attributes["connectionStringName"]) && !String.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]]))
            {
                _connectionString = System.Configuration.ConfigurationManager.AppSettings[objProvider.Attributes["connectionStringName"]];
            }
            else
            {
                _connectionString = objProvider.Attributes["connectionString"];
            }

            IDbConnection newConnection = new System.Data.SqlClient.SqlConnection();
            newConnection.ConnectionString = _connectionString.ToString();
            newConnection.Open();
            return newConnection;
        }

        #endregion

        #region Abstract methods

        public abstract DataTable GetDoctors(int Id = 0);
        public abstract DataTable GetPatients(int Id = 0);
        public abstract DataTable GetPatientLabs(int PatientId);
        public abstract DataTable GetLabDetails(int LabId);

        public abstract void SetPatient(string PersonalId, string Gender, DateTime Birthday, string FirstName, string SecondName, string LastName, string SecondLastName, string Phone,
                                        string CellPhone, string Whatsapp, string Email, string Address, int Id = 0);
        public abstract int SetLaboratory(int PatientId, int DoctorId, string LabName, string Observation, int UserId, int Id = 0);
        public abstract void SetLaboratoryDetail(int LaboratoryId, string LabDetail, string LabResult, string LabReference, string LabUnit, bool Remark, int UserId, int Id = 0);
        public abstract void SetClosedLaboratory(int Id, int UserId);
        public abstract void SetObservation(int Id, string Observation, int UserId);

        public abstract void DeleteLaboratory(int Id);
        public abstract void DeleteLaboratoryDetail(int Id);

        #region Users
        public abstract int CheckUserInSchedule(int user_id, string dayOfweek, TimeSpan currentTime);

        public abstract int GetBadLogonCount(string userName, string Ip);

        public abstract void SetLoginLog(string ip, string userName, string status);

        public abstract bool CheckIpBanned(string ip);

        public abstract DataTable GetSchedules(int scheduleId = 0);

        public abstract DataTable GetUsersSchedules(int user_id = 0);

        public abstract DataTable GetSchedulesDays();

        public abstract int SetSchedule(string description, int id = 0);

        public abstract int SetScheduleLimits(int scheduleId, int dayId, TimeSpan timeStart, TimeSpan timeEnd);

        public abstract int DeleteScheduleLimits(int scheduleId, int dayId);

        public abstract int SetUserSchedule(int user_id, int sch_id);

        public abstract int DeleteSchedule(int Id);

        public abstract DataTable GetBannedIP();

        public abstract int SetBannedIP(string Ip, string userName, string description);

        public abstract int DeleteBannedIP(string Ip);
        #endregion
        #endregion
    }
}