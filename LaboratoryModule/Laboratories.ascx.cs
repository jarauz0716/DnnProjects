using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jar.Dnn.LaboratoryModule
{
    public partial class Laboratories : LaboratoryModuleModuleBase
    {
        #region Variables Declaration

        int RowIndex = -1;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            AjaxMan = AjaxManager;

            if (!Page.IsPostBack)
            {
                CmbPatientsLabs.DataSource = Data.DataProvider.Instance().GetPatients();
                CmbPatientsLabs.DataBind();
            }
        }

        #region Laboratories
        protected void CmbPatientsLabs_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                System.Data.DataTable dt = Data.DataProvider.Instance().GetPatients(int.Parse(CmbPatientsLabs.SelectedValue));

                foreach (System.Data.DataRow drow in dt.Rows)
                {
                    LblPatPersonalId.Text = drow["PersonalId"].ToString();
                    LblPatName.Text = drow["DisplayName"].ToString();
                    LblPatGender.Text = drow["Gender_Desc"].ToString();
                    LblPatYears.Text = drow["Years"].ToString();
                    LblPatPhone.Text = drow["Phone"].ToString();
                    LblPatCellPhone.Text = drow["CellPhone"].ToString();
                    LblPatWhatsapp.Text = drow["Whatsapp"].ToString();
                    LblPatEmail.Text = drow["Email"].ToString();
                }

                GrdPatientLabs.Rebind();
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);
            }
        }

        protected void GrdPatientLabs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            int PatientId = 0;

            if (!string.IsNullOrEmpty(CmbPatientsLabs.SelectedValue))
                PatientId = int.Parse(CmbPatientsLabs.SelectedValue);

            GrdPatientLabs.DataSource = Data.DataProvider.Instance().GetPatientLabs(PatientId);
        }
        protected void GrdPatientLabs_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem && e.Item.IsInEditMode)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;

                    Telerik.Web.UI.GridEditManager editMan = editedItem.EditManager;

                    Telerik.Web.UI.GridDropDownListColumnEditor editor = (Telerik.Web.UI.GridDropDownListColumnEditor)(editMan.GetColumnEditor("LstDoctors"));
                    editor.DataSource = Data.DataProvider.Instance().GetDoctors();
                    editor.DataValueField = "Id";
                    editor.DataTextField = "DisplayName";
                    editor.DataBind();

                    editor.ComboBoxControl.Width = 400;
                    editor.ComboBoxControl.Font.Size = 10;
                    editor.ComboBoxControl.EmptyMessage = "Seleccione un Doctor";

                    if (editedItem.DataItem != null && editedItem.DataItem is System.Data.DataRowView)
                    {
                        System.Data.DataRowView dataItem = (System.Data.DataRowView)editedItem.DataItem;
                        if (dataItem != null)
                        {
                            editor.SelectedValue = dataItem["DoctorId"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogError(ex.Message);
                    DisplayAlert(ex.Message);
                }
            }

            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem dataItem = (Telerik.Web.UI.GridDataItem)e.Item;
                string status = dataItem["Status"].Text;
                string LabName = dataItem["LabName"].Text;

                Telerik.Web.UI.ElasticButton editControl = (Telerik.Web.UI.ElasticButton)dataItem["EditColumn"].Controls[0];
                Telerik.Web.UI.ElasticButton deleteControl = (Telerik.Web.UI.ElasticButton)dataItem["DeleteColumn"].Controls[0];

                deleteControl.ToolTip = "Eliminar";

                if (status == "Procesado")
                {
                    editControl.Enabled = false;
                    editControl.ForeColor = System.Drawing.Color.LightGray;
                    deleteControl.Enabled = false;
                    deleteControl.ForeColor = System.Drawing.Color.LightGray;
                }
                else
                {
                    deleteControl.Attributes["onclick"] = "if (!confirm('¿Desea borrar el laboratorio " + LabName + "? Se borraran todos los datos relacionados al mismo')) {return false;}";
                }
            }

            if (e.Item is Telerik.Web.UI.GridCommandItem)
            {
                Telerik.Web.UI.GridCommandItem cmditm = (Telerik.Web.UI.GridCommandItem)e.Item;
                System.Web.UI.WebControls.Button btn1 = (System.Web.UI.WebControls.Button)cmditm.FindControl("AddNewRecordButton");

                if (string.IsNullOrEmpty(CmbPatientsLabs.SelectedValue))
                    btn1.Visible = false;
                else
                    btn1.Visible = true;
            }
        }
        protected void GrdPatientLabs_PreRender(object sender, EventArgs e)
        {
            if (RowIndex > -1)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)GrdPatientLabs.Items[RowIndex];
                item.Selected = true;
                RowIndex = -1;
                GrdLabDetails.Rebind();
            }
        }
        protected void GrdPatientLabs_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item != null)
            {
                if (e.Item.ItemIndex >= 0)
                {
                    string Status = string.Empty;
                    string Observation = string.Empty;

                    e.Item.Selected = true;

                    foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                    {
                        Status = item["Status"].Text;
                        TxtObservation.Text = item["Observation"].Text.Replace("&nbsp;", "");

                        if (Status == "Procesado")
                        {
                            TxtObservation.Enabled = false;
                            BtnSaveObservation.Enabled = false;
                            BtnProcess.Enabled = false;
                            BtnExport.Enabled = true;
                        }
                        else
                        {
                            TxtObservation.Enabled = true;
                            BtnSaveObservation.Enabled = true;
                            BtnProcess.Enabled = true;
                            BtnExport.Enabled = false;
                        }
                    }
                }
                else
                {
                    if (e.CommandName == "RebindGrid")
                    {
                        GrdPatientLabs.Rebind();
                        GrdLabDetails.Rebind();
                        TxtObservation.Text = string.Empty;
                        TxtObservation.Enabled = false;
                        BtnSaveObservation.Enabled = false;
                        BtnProcess.Enabled = false;
                        BtnExport.Enabled = false;
                    }
                }
            }

            GrdLabDetails.Rebind();
        }
        protected void GrdPatientLabs_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    if (string.IsNullOrEmpty(CmbPatientsLabs.SelectedValue))
                    {
                        DisplayAlert("Debe seleccionar un paciente primero.", "Alerta", "Alert");
                        e.Canceled = true;
                        return;
                    }

                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int PatientId = int.Parse(CmbPatientsLabs.SelectedValue);
                    int DoctorId = 0;
                    int Id = 0;
                    string LabName = string.Empty;

                    System.Collections.Hashtable newValues = new System.Collections.Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    foreach (System.Collections.DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "DoctorId":
                                DoctorId = item.Value != null ? int.Parse(item.Value.ToString()) : 0;
                                break;
                            case "LabName":
                                LabName = item.Value.ToString();
                                break;
                        }
                    }

                    if (string.IsNullOrEmpty(CmbPatientsLabs.SelectedValue))
                    {
                        DisplayAlert("Debe seleccionar un doctor.", "Alerta", "Alert");
                        return;
                    }

                    Id = Data.DataProvider.Instance().SetLaboratory(PatientId, DoctorId, LabName, string.Empty, UserId);
                    GrdPatientLabs.Rebind();
                    RowIndex = 0;
                    TxtObservation.Text = string.Empty;
                    DisplayAlert("Laboratorio creado correctamente, puede proceder a agregar los detalles del mismo.", "Laboratorio Creado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }
            }
        }
        protected void GrdPatientLabs_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int Id = 0;
                    string LabName = string.Empty;
                    int DoctorId = 0;
                    string Observation = string.Empty;

                    var objId = editedItem.GetDataKeyValue("Id");

                    Observation = GrdPatientLabs.Items[editedItem.ItemIndex]["Observation"].Text.Replace("&nbsp;", string.Empty);

                    Id = objId != null ? (int)objId : 0;

                    System.Collections.Hashtable newValues = new System.Collections.Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    foreach (System.Collections.DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "LabName":
                                LabName = item.Value.ToString();
                                break;
                            case "DoctorId":
                                DoctorId = item.Value != null ? int.Parse(item.Value.ToString()) : 0;
                                break;
                            case "Observation":
                                Observation = item.Value.ToString();
                                break;
                        }
                    }

                    if (DoctorId == 0)
                    {
                        DisplayAlert("Debe seleccionar un medico", "Alerta", "Alert");
                        return;
                    }

                    Data.DataProvider.Instance().SetLaboratory(0, DoctorId, LabName, Observation, UserId, Id);
                    TxtObservation.Text = Observation;
                    GrdPatientLabs.Rebind();
                    GrdLabDetails.Rebind();
                    BtnSaveObservation.Enabled = false;
                    BtnProcess.Enabled = false;

                    DisplayAlert("Laboratorio actualizado correctamente", "Laboratorio Actualizado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }

            }
        }
        protected void GrdPatientLabs_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    var ObjId = editedItem.GetDataKeyValue("Id");

                    int Id = ObjId != null ? (int)ObjId : 0;

                    Data.DataProvider.Instance().DeleteLaboratory(Id);

                    DisplayAlert("El exámen de laboratorio fue eliminado correctamente", "Eliminar Exámen", "Info");
                    GrdPatientLabs.Rebind();
                    GrdLabDetails.Rebind();
                    TxtObservation.Text = string.Empty;
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }
            }
        }

        protected void GrdLabDetails_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            int LabId = 0;

            foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                LabId = int.Parse(item["Id"].Text);

            GrdLabDetails.DataSource = Data.DataProvider.Instance().GetLabDetails(LabId);
        }
        protected void GrdLabDetails_PreRender(object sender, EventArgs e)
        {

        }
        protected void GrdLabDetails_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            string Status = string.Empty;

            foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                Status = item["Status"].Text.ToString();

            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem dataItem = (Telerik.Web.UI.GridDataItem)e.Item;

                ((Telerik.Web.UI.ElasticButton)dataItem["DeleteColumn"].Controls[0]).ToolTip = "Eliminar";

                if (Status == "Procesado")
                {
                    ((Telerik.Web.UI.ElasticButton)dataItem["EditColumn"].Controls[0]).Enabled = false;
                    ((Telerik.Web.UI.ElasticButton)dataItem["EditColumn"].Controls[0]).ForeColor = System.Drawing.Color.LightGray;
                    ((Telerik.Web.UI.ElasticButton)dataItem["DeleteColumn"].Controls[0]).Enabled = false;
                    ((Telerik.Web.UI.ElasticButton)dataItem["DeleteColumn"].Controls[0]).ForeColor = System.Drawing.Color.LightGray;
                }
            }

            if (e.Item is Telerik.Web.UI.GridCommandItem)
            {
                Telerik.Web.UI.GridCommandItem cmditm = (Telerik.Web.UI.GridCommandItem)e.Item;
                System.Web.UI.WebControls.Button btn1 = (System.Web.UI.WebControls.Button)cmditm.FindControl("AddNewRecordButton");

                if (Status == "Procesado" || string.IsNullOrEmpty(Status))
                    btn1.Visible = false;
                else
                    btn1.Visible = true;
            }
        }
        protected void GrdLabDetails_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    int LabId = 0;
                    string LabDetail = string.Empty;
                    string LabResult = string.Empty;
                    string LabReference = string.Empty;
                    string LabUnit = string.Empty;
                    bool Remark = false;

                    foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                        LabId = int.Parse(item["Id"].Text);

                    if (LabId == 0)
                    {
                        DisplayAlert("Debe seleccionar un laboratorio primero.", "Alerta", "Alert");
                        e.Canceled = true;
                        return;
                    }

                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;

                    System.Collections.Hashtable newValues = new System.Collections.Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    foreach (System.Collections.DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "LabDetail":
                                LabDetail = item.Value.ToString();
                                break;
                            case "LabResult":
                                LabResult = item.Value.ToString();
                                break;
                            case "LabReference":
                                LabReference = item.Value.ToString();
                                break;
                            case "LabUnit":
                                LabUnit = item.Value.ToString();
                                break;
                            case "Remark":
                                Remark = bool.Parse(item.Value.ToString());
                                break;
                        }
                    }

                    Data.DataProvider.Instance().SetLaboratoryDetail(LabId, LabDetail, LabResult, LabReference, LabUnit, Remark, UserId);
                    GrdLabDetails.Rebind();
                    DisplayAlert("Resultado agregado correctamente", "Laboratorio Agregado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }
            }
        }
        protected void GrdLabDetails_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int Id = 0;
                    string LabDetail = string.Empty;
                    string LabResult = string.Empty;
                    string LabReference = string.Empty;
                    string LabUnit = string.Empty;
                    bool Remark = false;

                    var objId = editedItem.GetDataKeyValue("Id");

                    Id = objId != null ? (int)objId : 0;

                    System.Collections.Hashtable newValues = new System.Collections.Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    foreach (System.Collections.DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "LabDetail":
                                LabDetail = item.Value.ToString();
                                break;
                            case "LabResult":
                                LabResult = item.Value.ToString();
                                break;
                            case "LabReference":
                                LabReference = item.Value.ToString();
                                break;
                            case "LabUnit":
                                LabUnit = item.Value.ToString();
                                break;
                            case "Remark":
                                Remark = bool.Parse(item.Value.ToString());
                                break;
                        }
                    }


                    Data.DataProvider.Instance().SetLaboratoryDetail(0, LabDetail, LabResult, LabReference, LabUnit, Remark, UserId, Id);
                    GrdLabDetails.Rebind();

                    DisplayAlert("Resultado actualizado correctamente", "Resultado Actualizado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }

            }
        }
        protected void GrdLabDetails_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    var ObjId = editedItem.GetDataKeyValue("Id");

                    int Id = ObjId != null ? (int)ObjId : 0;

                    Data.DataProvider.Instance().DeleteLaboratoryDetail(Id);

                    DisplayAlert("Resultado eliminado correctamente", "Eliminar Resultado", "Info");
                    GrdLabDetails.Rebind();
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }
            }
        }

        protected void BtnClearResult_Click(object sender, EventArgs e)
        {
            GrdPatientLabs.Rebind();
            GrdLabDetails.Rebind();
            TxtObservation.Text = string.Empty;
            TxtObservation.Enabled = false;

            BtnSaveObservation.Enabled = false;
            BtnProcess.Enabled = false;
            BtnExport.Enabled = false;
        }
        protected void BtnProcess_Click(object sender, EventArgs e)
        {
            try
            {
                int LabId = 0;

                foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                    LabId = int.Parse(item["Id"].Text);

                if (LabId == 0)
                {
                    DisplayAlert("Debe seleccionar primero un laboratorio", "Seleccionar Laboratorio", "Alert");
                    return;
                }

                Data.DataProvider.Instance().SetClosedLaboratory(LabId, UserId);

                GrdPatientLabs.Rebind();
                GrdLabDetails.Rebind();
                TxtObservation.Text = string.Empty;
                TxtObservation.Enabled = false;
                BtnSaveObservation.Enabled = false;
                BtnProcess.Enabled = false;

                DisplayAlert("Exámen de laboratorio procesado correctamente.", "Exámen Procesado", "Info");
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);
            }
        }
        protected void BtnSaveObservation_Click(object sender, EventArgs e)
        {
            try
            {
                int LabId = 0;

                foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                    LabId = int.Parse(item["Id"].Text);

                if (LabId == 0)
                {
                    DisplayAlert("Debe seleccionar primero un laboratorio", "Seleccionar Laboratorio", "Alert");
                    return;
                }

                Data.DataProvider.Instance().SetObservation(LabId, TxtObservation.Text, UserId);
                DisplayAlert("Observación modificada correctamente.", "Modificar Observación", "Info");
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);
            }
        }

        #endregion
    }
}