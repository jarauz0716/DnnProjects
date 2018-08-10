using DotNetNuke.Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jar.Dnn.LaboratoryModule
{
    public partial class Doctors : LaboratoryModuleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AjaxMan = AjaxManager;
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        #region Doctors
        protected void GrdDoctors_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GrdDoctors.DataSource = Data.DataProvider.Instance().GetDoctors();
        }

        protected void GrdDoctors_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item != null)
            {
                if (e.Item.ItemIndex >= 0)
                {
                    int Id = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
                    switch (e.CommandName)
                    {
                        case "ViewData":
                            System.Data.DataTable dt = Data.DataProvider.Instance().GetDoctors(Id);

                            foreach (System.Data.DataRow drow in dt.Rows)
                            {
                                TxtDrPersonalId.Text = drow["PersonalId"].ToString();
                                CmbDrGender.SelectedValue = drow["Gender"].ToString();
                                DP_DrBirthday.SelectedDate = DateTime.Parse(drow["Birthday"].ToString());
                                TxtDrFirstName.Text = drow["FirstName"].ToString();
                                TxtDrSecondName.Text = drow["SecondName"].ToString();
                                TxtDrLastName.Text = drow["LastName"].ToString();
                                TxtDrSecondLastName.Text = drow["SecondLastName"].ToString();
                                TxtDrCode.Text = drow["DrCode"].ToString();
                                TxtDrRegistry.Text = drow["DrRegistry"].ToString();
                                TxtDrSpecialty.Text = drow["Specialty"].ToString();
                                TxtDrPhone.Text = drow["Phone"].ToString();
                                TxtDrCellPhone.Text = drow["CellPhone"].ToString();
                                TxtDrWhatsapp.Text = drow["Whatsapp"].ToString();
                                TxtDrEmail.Text = drow["Email"].ToString();
                                TxtDrAddress.Text = drow["Address"].ToString();
                            }

                            break;
                        default:
                            GrdDoctors.Rebind();
                            break;
                    }

                    e.Item.Selected = true;
                }
            }
        }

        protected void BtnDrSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtDrPersonalId.Text) || string.IsNullOrEmpty(CmbDrGender.SelectedValue) || DP_DrBirthday.SelectedDate == null ||
                    string.IsNullOrEmpty(TxtDrFirstName.Text) || string.IsNullOrEmpty(TxtDrLastName.Text) || string.IsNullOrEmpty(TxtDrEmail.Text) ||
                    string.IsNullOrEmpty(TxtDrCode.Text) || string.IsNullOrEmpty(TxtDrRegistry.Text) || string.IsNullOrEmpty(TxtDrSpecialty.Text))
                {
                    DisplayAlert("Los campos marcados con asterisco (*) son requeridos", "Alerta", "Alert");
                    return;
                }

                int Id = 0;

                foreach (Telerik.Web.UI.GridDataItem item in GrdDoctors.SelectedItems)
                    Id = int.Parse(item["Id"].Text.ToString());

                DateTime BirthDay = DP_DrBirthday.SelectedDate ?? DateTime.MinValue;
                Data.DataProvider.Instance().SetDoctor(TxtDrPersonalId.Text, CmbDrGender.SelectedValue, BirthDay, TxtDrFirstName.Text, TxtDrSecondName.Text, TxtDrLastName.Text, 
                                                       TxtDrSecondLastName.Text, TxtDrCode.Text, TxtDrRegistry.Text, TxtDrSpecialty.Text,
                                                       TxtDrPhone.Text, TxtDrCellPhone.Text, TxtDrWhatsapp.Text, TxtDrEmail.Text, TxtDrAddress.Text, Id);

                if (Id == 0)
                {
                    ClearDrItems();
                    DisplayAlert("Doctor registrado correctamente", "Registro Doctor", "Info");
                }
                else
                {
                    DisplayAlert("Doctor actualizado correctamente", "Registro Doctor", "Info");
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);
            }
        }

        protected void BtnDrClear_Click(object sender, EventArgs e)
        {
            ClearDrItems();
        }


        protected void ClearDrItems()
        {
            TxtDrPersonalId.Text = string.Empty;
            CmbDrGender.ClearSelection();
            DP_DrBirthday.Clear();
            TxtDrFirstName.Text = string.Empty;
            TxtDrSecondName.Text = string.Empty;
            TxtDrLastName.Text = string.Empty;
            TxtDrSecondLastName.Text = string.Empty;
            TxtDrCode.Text = string.Empty;
            TxtDrRegistry.Text = string.Empty;
            TxtDrSpecialty.Text = string.Empty;
            TxtDrPhone.Text = string.Empty;
            TxtDrCellPhone.Text = string.Empty;
            TxtDrWhatsapp.Text = string.Empty;
            TxtDrEmail.Text = string.Empty;
            TxtDrAddress.Text = string.Empty;

            GrdDoctors.Rebind();
        }

        #endregion
    }
}