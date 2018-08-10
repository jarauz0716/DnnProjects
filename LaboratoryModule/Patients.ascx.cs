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

using System;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;

namespace Jar.Dnn.LaboratoryModule
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from LaboratoryModuleModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class Patients : LaboratoryModuleModuleBase
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

        #region Patients
        protected void GrdPatients_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            GrdPatients.DataSource = Data.DataProvider.Instance().GetPatients();
        }
        protected void GrdPatients_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }
        protected void GrdPatients_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item != null)
            {
                if (e.Item.ItemIndex >= 0)
                {
                    int Id = int.Parse(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["Id"].ToString());
                    switch (e.CommandName)
                    {
                        case "ViewData":
                            System.Data.DataTable dt = Data.DataProvider.Instance().GetPatients(Id);

                            foreach (System.Data.DataRow drow in dt.Rows)
                            {
                                TxtPersonalId.Text = drow["PersonalId"].ToString();
                                CmbGender.SelectedValue = drow["Gender"].ToString();
                                DP_Birthday.SelectedDate = DateTime.Parse(drow["Birthday"].ToString());
                                TxtFirstName.Text = drow["FirstName"].ToString();
                                TxtSecondName.Text = drow["SecondName"].ToString();
                                TxtLastName.Text = drow["LastName"].ToString();
                                TxtSecondLastName.Text = drow["SecondLastName"].ToString();
                                TxtPhone.Text = drow["Phone"].ToString();
                                TxtCellPhone.Text = drow["CellPhone"].ToString();
                                TxtWhatsapp.Text = drow["Whatsapp"].ToString();
                                TxtEmail.Text = drow["Email"].ToString();
                                TxtAddress.Text = drow["Address"].ToString();
                            }

                            break;
                        default:
                            GrdPatients.Rebind();
                            break;
                    }

                    e.Item.Selected = true;
                }
            }
        }

        protected void BtnClear_Click(object sender, EventArgs e)
        {
            ClearItems();
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtPersonalId.Text) || string.IsNullOrEmpty(CmbGender.SelectedValue) || DP_Birthday.SelectedDate == null ||
                    string.IsNullOrEmpty(TxtFirstName.Text) || string.IsNullOrEmpty(TxtLastName.Text) || string.IsNullOrEmpty(TxtEmail.Text))
                {
                    DisplayAlert("Los campos marcados con asterisco (*) son requeridos", "Alerta", "Alert");
                    return;
                }

                int Id = 0;

                foreach (Telerik.Web.UI.GridDataItem item in GrdPatients.SelectedItems)
                    Id = int.Parse(item["Id"].Text.ToString());

                DateTime BirthDay = DP_Birthday.SelectedDate ?? DateTime.MinValue;
                Data.DataProvider.Instance().SetPatient(TxtPersonalId.Text, CmbGender.SelectedValue, BirthDay, TxtFirstName.Text, TxtSecondName.Text, TxtLastName.Text, TxtSecondLastName.Text,
                                                        TxtPhone.Text, TxtCellPhone.Text, TxtWhatsapp.Text, TxtEmail.Text, TxtAddress.Text, Id);

                if (Id == 0)
                {
                    ClearItems();
                    DisplayAlert("Paciente registrado correctamente", "Registro Paciente", "Info");
                }
                else
                {
                    DisplayAlert("Paciente actualizado correctamente", "Registro Paciente", "Info");
                }

            }
            catch (Exception ex)
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
        }

        protected void ClearItems()
        {
            TxtPersonalId.Text = string.Empty;
            CmbGender.ClearSelection();
            DP_Birthday.Clear();
            TxtFirstName.Text = string.Empty;
            TxtSecondName.Text = string.Empty;
            TxtLastName.Text = string.Empty;
            TxtSecondLastName.Text = string.Empty;
            TxtPhone.Text = string.Empty;
            TxtCellPhone.Text = string.Empty;
            TxtWhatsapp.Text = string.Empty;
            TxtEmail.Text = string.Empty;
            TxtAddress.Text = string.Empty;

            GrdPatients.Rebind();
        }

        #endregion
    }
}