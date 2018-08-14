#region Copyright

// 
// Copyright (c) 2018
// by Jar.Dnn
// 

#endregion

#region Using Statements

using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using System.Collections;

#endregion

namespace Jar.Dnn.LaboratoryModule
{

    public partial class Laboratory : LaboratoryPortalModuleBase
    {

        #region Variables Declaration

        int RowIndex = -1;

        #endregion

        #region Event Handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AjaxMan = AjaxManager;

            if (!Page.IsPostBack)
            {
                CmbPatientsLabs.DataSource = Data.DataProvider.Instance().GetPatients();
                CmbPatientsLabs.DataBind();

                System.Data.DataTable dtSchedules = Data.DataProvider.Instance().GetSchedules();
                System.Data.DataTable dtUsers = LoadUsers();
                

                CmbDnnUsers.DataSource = dtUsers;
                CmbDnnUsers.DataTextField = "USERNAME";
                CmbDnnUsers.DataValueField = "USERID";
                CmbDnnUsers.DataBind();

                //LocalizateRadGrid(ref GrdUsers);

                GrdUsers.DataSource = dtUsers;
                GrdUsers.DataBind();

                GrdUsers_Roles.DataSource = ConstructTable("ROLES_USER");
                GrdUsers_Roles.DataBind();
                GrdUsers_Roles.Culture = new System.Globalization.CultureInfo("es-ES");

            }
        }

        #endregion

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
                    BtnSaveObservation.Enabled = true;
                    BtnProcess.Enabled = true;
                    BtnExport.Enabled = false;
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

            if (e.Item is Telerik.Web.UI.GridEditFormInsertItem && e.Item.OwnerTableView.IsItemInserted)
            {
                GrdLabDetails.MasterTableView.GetItems(Telerik.Web.UI.GridItemType.FilteringItem)[0].Visible = false;
            }

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
                                LabReference = item.Value != null ? item.Value.ToString() : string.Empty;
                                break;
                            case "LabUnit":
                                LabUnit = item.Value != null ? item.Value.ToString() : string.Empty;
                                break;
                            case "Remark":
                                Remark = bool.Parse(item.Value.ToString());
                                break;
                        }
                    }

                    Data.DataProvider.Instance().SetLaboratoryDetail(LabId, LabDetail, LabResult, LabReference, LabUnit, Remark, UserId);
                    GrdLabDetails.Rebind();
                    //DisplayAlert("Resultado agregado correctamente", "Laboratorio Agregado", "Info");
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
                                LabReference = item.Value != null ? item.Value.ToString() : string.Empty;
                                break;
                            case "LabUnit":
                                LabUnit = item.Value != null ? item.Value.ToString() : string.Empty;
                                break;
                            case "Remark":
                                Remark = bool.Parse(item.Value.ToString());
                                break;
                        }
                    }


                    Data.DataProvider.Instance().SetLaboratoryDetail(0, LabDetail, LabResult, LabReference, LabUnit, Remark, UserId, Id);
                    GrdLabDetails.Rebind();

                    //DisplayAlert("Resultado actualizado correctamente", "Resultado Actualizado", "Info");
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

        protected void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                int LabId = 0;
                string Status = string.Empty;
                string PatientName = string.Empty;
                string LabName = string.Empty;
                System.Data.DataTable dt = new System.Data.DataTable();

                if (GrdPatientLabs.SelectedItems.Count == 0)
                {
                    DisplayAlert("Debe seleccionar primero un laboratorio para poder generar.", "Alerta", "Alert");
                    return;
                }

                foreach (Telerik.Web.UI.GridDataItem item in GrdPatientLabs.SelectedItems)
                {
                    LabId = int.Parse(item["Id"].Text);
                    Status = item["Status"].Text;
                    PatientName = item["Patient"].Text;
                    LabName = item["LabName"].Text;
                }

                if (Status != "Procesado")
                {
                    DisplayAlert("Debe seleccionar un laboratorio procesado para poder generar.", "Alerta", "Alert");
                    return;
                }


                Telerik.Reporting.Processing.RenderingResult result;
                Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();
                Telerik.Reporting.TypeReportSource reportSource = new Telerik.Reporting.TypeReportSource
                {
                    TypeName = typeof(LabsLibrary.RptLab).AssemblyQualifiedName
                };
                
                dt = Data.DataProvider.Instance().GetLaboratory(LabId);
                reportSource.Parameters.Add("XmlLaboratory", APC.Utility.Functions.Util.DataTableToXml(dt));

                dt = Data.DataProvider.Instance().GetLabDetails(LabId);
                reportSource.Parameters.Add("XmlLaboratoryDetails", APC.Utility.Functions.Util.DataTableToXml(dt));

                result = reportProcessor.RenderReport("PDF", reportSource, null);

                string fileName = string.Format("{0} - {1}.", LabName, PatientName) + result.Extension;

                Response.Clear();
                Response.ContentType = result.MimeType;
                Response.Cache.SetCacheability(System.Web.HttpCacheability.Private);
                Response.Expires = -1;
                Response.Buffer = true;

                Response.AddHeader("Content-Disposition", string.Format("{0};FileName=\"{1}\"", "attachment", fileName));

                Response.BinaryWrite(result.DocumentBytes);
                Response.End();
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);
            }
        }

        #endregion

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
                Data.DataProvider.Instance().SetPatient(TxtPersonalId.Text, CmbGender.SelectedValue, BirthDay, TxtFirstName.Text, TxtSecondName.Text, TxtLastName.Text, TxtSecondLastName.Text,
                                                        TxtPhone.Text, TxtCellPhone.Text, TxtWhatsapp.Text, TxtEmail.Text, TxtAddress.Text, Id);

                if (Id == 0)
                {
                    ClearItems();
                    DisplayAlert("Doctor registrado correctamente", "Registro Doctor", "Info");
                }
                else
                {
                    DisplayAlert("Doctor actualizado correctamente", "Registro Doctor", "Info");
                }
            }
            catch(Exception ex)
            {
                DisplayAlert(ex);
            }
        }

        protected void BtnDrClear_Click(object sender, EventArgs e)
        {

        }


        protected void ClearDrItems()
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

        #region Users

        protected void GrdUsers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            System.Data.DataTable dtUsers = new System.Data.DataTable();

            //LocalizateRadGrid(ref GrdUsers);

            try
            {
                dtUsers = LoadUsers();
            }
            catch
            {
                dtUsers = ConstructTable("USERS");
            }

            GrdUsers.DataSource = dtUsers;
        }
        protected void GrdUsers_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridNestedViewItem)
            {
                e.Item.FindControl("InnerContainer").Visible = ((Telerik.Web.UI.GridNestedViewItem)e.Item).ParentItem.Expanded;
            }
        }
        protected void GrdUsers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem && e.Item.IsInEditMode)
            {
                try
                {
                    if (!e.Item.OwnerTableView.IsItemInserted)
                    {
                        Telerik.Web.UI.GridEditFormItem editItem = (Telerik.Web.UI.GridEditFormItem)e.Item;
                        editItem["PASSWORD"].Parent.Visible = false;
                    }
                    else
                    {
                        Telerik.Web.UI.GridEditableItem editItem = (Telerik.Web.UI.GridEditableItem)e.Item;
                        ((System.Web.UI.WebControls.TextBox)editItem["PASSWORD"].Controls[0]).TextMode = System.Web.UI.WebControls.TextBoxMode.Password;
                    }

                    if (!e.Item.OwnerTableView.IsItemInserted)
                    {
                        Telerik.Web.UI.GridEditFormItem editItem = (Telerik.Web.UI.GridEditFormItem)e.Item;
                        editItem["USERNAME"].Parent.Visible = false;
                    }

                    CollapseAllRows(GrdUsers, -1);
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                }
                catch (Exception ex)
                {
                    DisplayAlert(ex);
                }

            }
            else
            {
                if (e.Item is Telerik.Web.UI.GridDataItem)
                {
                    Telerik.Web.UI.GridDataItem dataItem = e.Item as Telerik.Web.UI.GridDataItem;

                    if (dataItem["AUTHORIZED"].Text == "No")
                    {
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("Authorized")).CssClass = "RadAButton rbLink rbIcon rbNotAuthorized";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("Authorized")).ToolTip = "Usuario Desautorizado - ¿Autorizar Usuario?";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("Authorized")).Attributes.Add("onClick", "onAuthorizedUser('" + dataItem.ItemIndex.ToString() + "');");
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("Authorized")).CssClass = "RadAButton rbLink rbIcon rbAuthorized";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("Authorized")).ToolTip = "Usuario Autorizado - ¿Desautorizar Usuario?";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("Authorized")).Attributes.Add("onClick", "onNotAuthorizedUser('" + dataItem.ItemIndex.ToString() + "');");
                    }


                    if (dataItem["LOCKED_OUT"].Text == "No")
                    {
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).CssClass = "RadAButton rbLink rbIcon rbNotLockedOut";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).ToolTip = "Usuario Desbloqueado";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).Enabled = false;
                    }
                    else
                    {
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).CssClass = "RadAButton rbLink rbIcon rbLockedOut";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).ToolTip = "Usuario Bloqueado - ¿Desbloquear Usuario?";
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).Attributes.Add("onClick", "onLockedOut('" + dataItem.ItemIndex.ToString() + "');");
                        ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("LockedOut")).Enabled = true;
                    }


                    ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("SendPassMail")).Attributes.Add("onClick", "onSendMailPass('" + dataItem.ItemIndex.ToString() + "');");
                    ((System.Web.UI.WebControls.LinkButton)dataItem["Actions"].FindControl("SetPassword")).Attributes.Add("onClick", "onSetPassword('" + dataItem.ItemIndex.ToString() + "');");
                    //((System.Web.UI.WebControls.LinkButton)dataItem["DelUsers"].FindControl("DeleteUser")).Attributes.Add("onClick", "onDeleteUser('" + dataItem.ItemIndex.ToString() + "');");
                }
            }
        }
        protected void GrdUsers_ItemUpdated(object sender, Telerik.Web.UI.GridUpdatedEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
            }
        }
        protected void GrdUsers_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                DnnUsers dnn = new DnnUsers();
                Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                string userName = editedItem.GetDataKeyValue("USERNAME").ToString();

                System.Data.DataTable dtUsers = new System.Data.DataTable();
                System.Data.DataTable dt = new System.Data.DataTable();
                int index = editedItem.ItemIndex;

                dtUsers = GetDataTable(GrdUsers);

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                UserInfo user = new UserInfo();
                user = UserController.GetUserByName(userName);

                foreach (DictionaryEntry item in newValues)
                {
                    switch (item.Key.ToString())
                    {
                        case "NAME":
                            if (user.FirstName != item.Value.ToString())
                            {
                                user.FirstName = item.Value.ToString();
                                user.Profile.FirstName = item.Value.ToString();
                            }
                            break;
                        case "LASTNAME":
                            if (user.LastName != item.Value.ToString())
                            {
                                user.LastName = item.Value.ToString();
                                user.Profile.LastName = item.Value.ToString();
                            }
                            break;
                        case "DISPLAYNAME":
                            if (user.DisplayName != item.Value.ToString())
                            {
                                user.DisplayName = item.Value.ToString();
                            }
                            break;
                        case "EMAIL":
                            if (user.Email != item.Value.ToString())
                            {
                                user.Email = item.Value.ToString();
                            }
                            break;
                    }
                }

                try
                {
                    UserController.UpdateUser(PortalId, user);
                    GrdUsers.MasterTableView.ClearEditItems();
                    DisplayAlert("Registro actualizado correctamente", "Registro Actualizado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    GrdUsers.MasterTableView.ClearEditItems();

                    DisplayAlert(ex);
                }

            }
        }
        protected void GrdUsers_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                DnnUsers dnn = new DnnUsers();
                Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                System.Text.RegularExpressions.Regex _rgx = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9-_]{1,100}$");

                Hashtable newValues = new Hashtable();
                e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                UserInfo user = new UserInfo()
                {
                    IsSuperUser = false,
                    PortalID = PortalId
                };

                string lc = new DotNetNuke.Services.Localization.Localization().CurrentUICulture;

                user.Profile.InitialiseProfile(PortalId);
                user.Profile.PreferredTimeZone = PortalSettings.TimeZone;
                user.Profile.PreferredLocale = lc;

                user.Membership.Approved = true;
                user.Membership.CreatedDate = DateTime.Now;
                user.Membership.IsOnLine = false;

                foreach (DictionaryEntry item in newValues)
                {
                    switch (item.Key.ToString())
                    {
                        case "USERNAME":
                            if (!_rgx.IsMatch(item.Value.ToString()))
                            {
                                e.Canceled = true;
                                DisplayAlert("Error: el usuario no tiene un formato válido, valores permitidos A-Z a-z 0-9 _ -");
                                return;
                            }
                            user.Username = item.Value.ToString();
                            break;
                        case "NAME":
                            user.FirstName = item.Value.ToString();
                            user.Profile.FirstName = item.Value.ToString();
                            break;
                        case "LASTNAME":
                            user.LastName = item.Value.ToString();
                            user.Profile.LastName = item.Value.ToString();
                            break;
                        case "DISPLAYNAME":
                            user.DisplayName = item.Value.ToString();
                            break;
                        case "EMAIL":
                            user.Email = item.Value.ToString();
                            break;
                        case "PASSWORD":
                            user.Membership.Password = item.Value.ToString();
                            break;
                    }
                }

                try
                {
                    DotNetNuke.Security.Membership.UserCreateStatus AddUserStatus = DotNetNuke.Security.Membership.UserCreateStatus.AddUser;
                    AddUserStatus = UserController.CreateUser(ref user);
                    string addmsg = string.Empty;
                    bool valid = false;

                    switch (AddUserStatus)
                    {
                        case DotNetNuke.Security.Membership.UserCreateStatus.Success:
                            valid = true;
                            addmsg = "Usuario creado correctamente.";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateEmail:
                            addmsg = "El correo ingresado ya se encuentra registrado en el sistema";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateUserName:
                            addmsg = "El usuario ingresado ya se encuentra registrado en el sistema";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.DuplicateProviderUserKey:
                            addmsg = "El key ingresado ya se encuentra registrado en el sistema";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.InvalidEmail:
                            addmsg = "El correo ingresado no es válido";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.InvalidPassword:
                            addmsg = "La contraseña ingresada no es válida";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.InvalidUserName:
                            addmsg = "El usuario ingresado no es válido";
                            break;
                        case DotNetNuke.Security.Membership.UserCreateStatus.UserRejected:
                            addmsg = "El usuario ingresado fue rechazado por el sistema";
                            break;
                        default:
                            addmsg = AddUserStatus.ToString();
                            break;
                    }

                    if (valid)
                    {
                        GrdUsers.MasterTableView.ClearEditItems();
                        GrdUsers.DataSource = LoadUsers();
                        GrdUsers.DataBind();
                        DisplayAlert("Usuario creado correctamente", "Usuario Creado", "Info");
                    }
                    else
                    {
                        e.Canceled = true;
                        DisplayAlert("Error: " + addmsg);
                    }
                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }

            }
        }
        protected void GrdUsers_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            System.Data.DataRowView dtItem = default(System.Data.DataRowView);
            System.Data.DataRow dtrow = default(System.Data.DataRow);
            Telerik.Web.UI.GridDataItem item;
            System.Data.DataTable dtGrd = new System.Data.DataTable();

            if (e.CommandName == "RowClick" || e.CommandName == "ExpandCollapse")
            {
                bool lastState = e.Item.Expanded;

                lastState = !lastState;

                CollapseAllRows(GrdUsers, e.Item.ItemIndex);
                e.Item.Expanded = !lastState;

                if (lastState)
                {
                    System.Data.DataTable dt = GetDataTable(GrdUsers);
                    int id = int.Parse(dt.Rows[e.Item.ItemIndex]["USERID"].ToString());
                    UserInfo usr = UserController.GetUserById(PortalId, id);

                    Telerik.Web.UI.GridDataItem parentItem = e.Item as Telerik.Web.UI.GridDataItem;
                    Telerik.Web.UI.RadGrid GrdRoles = parentItem.ChildItem.FindControl("GrdDetail_Roles") as Telerik.Web.UI.RadGrid;
                    GrdRoles.DataSource = LoadUserRoles(usr.UserID);
                    GrdRoles.DataBind();
                }
            }
            else
            {
                if (e.Item.DataItem != null || !string.IsNullOrEmpty(e.CommandArgument.ToString()))
                {
                    try
                    {
                        switch (e.CommandName)
                        {
                            case "SendPasswordMail":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());
                                    
                                    int L_UserID = int.Parse(GrdUsers.Items[itemIndex].GetDataKeyValue("USERID").ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);
                                    UserController.ResetPasswordToken(usrDnn);
                                    DotNetNuke.Services.Mail.Mail.SendMail(usrDnn, DotNetNuke.Services.Mail.MessageType.PasswordReminder, PortalSettings);
                                    DisplayAlert("Enlace de restablecimiento de contraseña enviado correctamente.", "Restablecer Contraseña", "Info");
                                }
                                break;
                            case "LockedOut":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());

                                    int L_UserID = int.Parse(GrdUsers.Items[itemIndex].GetDataKeyValue("USERID").ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);

                                    UserController.UnLockUser(usrDnn);

                                    DisplayAlert("Usuario desbloqueado correctamente.", "Desbloquear Usuario", "Info");

                                    GrdUsers.Rebind();
                                }
                                break;
                            case "Authorized":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());

                                    int L_UserID = int.Parse(GrdUsers.Items[itemIndex].GetDataKeyValue("USERID").ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);
                                    string tmpmessage = "";

                                    if (usrDnn.Membership.Approved)
                                        tmpmessage = "desautorizado";
                                    else
                                        tmpmessage = "autorizado";

                                    usrDnn.Membership.Approved = !usrDnn.Membership.Approved;

                                    UserController.UpdateUser(PortalId, usrDnn);

                                    DisplayAlert("Usuario " + tmpmessage + " correctamente.", "Autorizar Usuario", "Info");

                                    GrdUsers.Rebind();

                                }
                                break;
                            case "SetPassword":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    string[] arguments = e.CommandArgument.ToString().Split('|');
                                    string password = arguments[1];
                                    int itemIndex = int.Parse(arguments[0]);

                                    int L_UserID = int.Parse(GrdUsers.Items[itemIndex].GetDataKeyValue("USERID").ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);

                                    UserController.ResetPasswordToken(usrDnn, 2); // set to 2 minute tuim
                                    String passwordResetToken = usrDnn.PasswordResetToken.ToString();
                                    Boolean returnResult = false;

                                    returnResult = UserController.ChangePasswordByToken(usrDnn.PortalID, usrDnn.Username, password, passwordResetToken);

                                    DisplayAlert("Contraserña cambiada correctamente.", "Cambiar Contraseña", "Info");
                                }
                                break;
                            case "DeleteUsr":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());

                                    bool dnnDeleted = false;
                                    int L_UserID = int.Parse(GrdUsers.Items[itemIndex].GetDataKeyValue("USERID").ToString());
                                    UserInfo user = UserController.GetUserById(PortalId, L_UserID);

                                    if (user != null)
                                    {
                                        dnnDeleted = UserController.DeleteUser(ref user, false, false);
                                        dnnDeleted = UserController.RemoveUser(user);

                                        if (dnnDeleted)
                                        {
                                            DisplayAlert("Se eliminó el usuario correctamente.", "Eliminar Usuario", "Info");
                                        }
                                        else
                                        {
                                            DisplayAlert("El usuario no se pudo eliminar.", "Eliminar Usuario", "Alert");
                                        }

                                        GrdUsers.DataSource = LoadUsers();
                                        GrdUsers.DataBind();
                                    }
                                    else
                                    {
                                        DisplayAlert("El usuario no se pudo encontrar en el portal.", "Eliminar Usuario", "Alert");
                                    }
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        DisplayAlert(ex);
                    }
                }
            }
        }
        
        protected void GrdUsers_Roles_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            System.Data.DataTable dtRoles = new System.Data.DataTable();

            try
            {
                dtRoles = GetDataTable(GrdUsers_Roles);
            }
            catch
            {
                dtRoles = ConstructTable("ROLES_USER");
            }
            GrdUsers_Roles.DataSource = dtRoles;
        }
        protected void GrdUsers_Roles_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem && e.Item.IsInEditMode)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;

                    Telerik.Web.UI.GridEditManager editMan = editedItem.EditManager;
                    Telerik.Web.UI.GridDropDownListColumnEditor editor = (Telerik.Web.UI.GridDropDownListColumnEditor)(editMan.GetColumnEditor("lstRoles"));

                    editor.DataSource = LoadRoles();
                    editor.DataTextField = "ROL_NAME";
                    editor.DataValueField = "ROL_NAME";
                    editor.DataBind();

                    editor.ComboBoxControl.Width = System.Web.UI.WebControls.Unit.Pixel(150);

                    if (!e.Item.OwnerTableView.IsItemInserted)
                    {
                        System.Data.DataRowView dataItem = (System.Data.DataRowView)editedItem.DataItem;
                        if (dataItem != null)
                        {
                            editor.SelectedText = dataItem["ROL_NAME"].ToString();
                        }
                    }

                    Telerik.Web.UI.RadDatePicker dp = editedItem["EXPIRE_DATE"].Controls[0] as Telerik.Web.UI.RadDatePicker;
                    dp.Height = System.Web.UI.WebControls.Unit.Pixel(35);
                    dp.Culture = new System.Globalization.CultureInfo("es-ES");
                    dp.SharedCalendar.ShowRowHeaders = false;
                    dp.SharedCalendar.CultureInfo = new System.Globalization.CultureInfo("es-ES");
                    dp.SharedCalendar.Width = System.Web.UI.WebControls.Unit.Pixel(250);


                }
                catch (Exception ex)
                {
                    DisplayAlert(ex);
                    //DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, ex.Message + ex.StackTrace, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
                }

            }
        }
        protected void GrdUsers_Roles_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    if (string.IsNullOrEmpty(CmbDnnUsers.SelectedValue))
                    {
                        GrdUsers_Roles.MasterTableView.ClearChildSelectedItems();
                        GrdUsers_Roles.MasterTableView.ClearEditItems();
                        e.Canceled = true;
                        DisplayAlert("Error: Debe serleccionar primero un usuario.", "Alerta", "Alert");
                        return;
                    }

                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    string roleName = string.Empty;
                    int userId = int.Parse(CmbDnnUsers.SelectedValue.ToString());

                    System.Data.DataTable dtUserRoles = new System.Data.DataTable();

                    DotNetNuke.Security.Roles.RoleController rolController = new DotNetNuke.Security.Roles.RoleController();
                    UserInfo usr = UserController.GetUserById(PortalId, userId);
                    DotNetNuke.Security.Roles.RoleInfo rol = new DotNetNuke.Security.Roles.RoleInfo();
                    DateTime expireDate = new DateTime();

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    foreach (DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "ROL_NAME":
                                roleName = item.Value.ToString();
                                break;
                            case "EXPIRE_DATE":
                                if (item.Value == null)
                                    expireDate = DateTime.MaxValue;
                                else
                                    expireDate = DateTime.Parse(item.Value.ToString());
                                break;
                        }
                    }

                    rol = rolController.GetRoleByName(PortalId, roleName);

                    rolController.AddUserRole(PortalId, userId, rol.RoleID, DateTime.Now, expireDate);

                    GrdUsers_Roles.MasterTableView.ClearEditItems();
                    GrdUsers_Roles.DataSource = LoadUserRoles(userId);
                    GrdUsers_Roles.Rebind();
                    DisplayAlert("Rol agregado correctamente", "Agregar Rol", "Info");

                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    DisplayAlert(ex);
                }
            }
        }
        protected void GrdUsers_Roles_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int index = editedItem.ItemIndex;
                    string roleName = string.Empty;

                    System.Data.DataTable dtUserRoles = new System.Data.DataTable();
                    DotNetNuke.Security.Roles.RoleController rolController = new DotNetNuke.Security.Roles.RoleController();

                    dtUserRoles = GetDataTable(GrdUsers_Roles);

                    roleName = dtUserRoles.Rows[index]["ROL_NAME"].ToString();
                    UserInfo usr = UserController.GetUserById(PortalId, int.Parse(CmbDnnUsers.SelectedValue));
                    DotNetNuke.Security.Roles.RoleInfo rol = rolController.GetRoleByName(PortalId, roleName);

                    if (DotNetNuke.Security.Roles.RoleController.DeleteUserRole(usr, rol, PortalSettings, false))
                    {
                        GrdUsers_Roles.MasterTableView.ClearEditItems();
                        GrdUsers_Roles.DataSource = LoadUserRoles(usr.UserID);
                        GrdUsers_Roles.Rebind();
                        DisplayAlert("Rol eliminado correctamente", "Eliminar Rol", "Info");
                    }
                    else
                    {
                        e.Canceled = true;
                        DisplayAlert("El rol no se pudo eliminar.", "Alerta", "Alert");
                    }
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    DisplayAlert(ex);
                }
            }

        }
        
        protected void CmbDnnUsers_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                int _UserId = int.Parse(e.Value);

                GrdUsers_Roles.DataSource = LoadUserRoles(_UserId);
                GrdUsers_Roles.DataBind();
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);
                GrdUsers_Roles.DataSource = ConstructTable("ROLES_USER");
                GrdUsers_Roles.DataBind();
            }
        }

        private void CollapseAllRows(Telerik.Web.UI.RadGrid rgd, int itemIndex)
        {
            foreach (Telerik.Web.UI.GridItem item in rgd.MasterTableView.Items)
            {
                if (item.ItemIndex != itemIndex)
                    item.Expanded = false;
            }
        }

        private System.Data.DataTable LoadUsers()
        {
            System.Data.DataTable dt;
            System.Data.DataRow drow;
            System.Collections.ArrayList lstDnnUsers;

            dt = ConstructTable("USERS");

            try
            {
                lstDnnUsers = UserController.GetUsers(PortalId);

                foreach (UserInfo usrInfo in lstDnnUsers)
                {
                    drow = dt.NewRow();

                    drow["USERID"] = usrInfo.UserID;
                    drow["USERNAME"] = usrInfo.Username;
                    drow["DISPLAYNAME"] = usrInfo.DisplayName;
                    drow["NAME"] = usrInfo.FirstName;
                    drow["LASTNAME"] = usrInfo.LastName;
                    drow["EMAIL"] = usrInfo.Email;
                    drow["PASSWORD"] = "";
                    drow["AUTHORIZED"] = usrInfo.Membership.Approved ? "Sí" : "No";
                    drow["LOCKED_OUT"] = usrInfo.Membership.LockedOut ? "Sí" : "No"; ;
                    drow["IS_ONLINE"] = usrInfo.Membership.IsOnLine ? "Sí" : "No"; ;

                    dt.Rows.Add(drow);
                }

            }
            catch (Exception ex)
            {
                dt = ConstructTable("USERS");

                DisplayAlert(ex);

            }

            return dt;
        }

        private System.Data.DataTable LoadUserRoles(int userId)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            try
            {
                dt = ConstructTable("ROLES_USER");
                UserInfo user = UserController.GetUserById(PortalId, userId);
                DotNetNuke.Security.Roles.RoleController rolCon = new DotNetNuke.Security.Roles.RoleController();
                //RoleInfo rol = new RoleInfo();

                //rolCon.GetUser

                foreach (UserRoleInfo rol in rolCon.GetUserRoles(user, true))
                {
                    dt.Rows.Add(rol.RoleID, rol.RoleName, rol.EffectiveDate, rol.ExpiryDate);
                }
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);

            }

            return dt;
        }

        private System.Data.DataTable LoadRoles()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt.Columns.Add("ROL_NAME");
            System.Data.DataRow drow;

            System.Collections.Generic.IList<DotNetNuke.Security.Roles.RoleInfo> roles;

            DotNetNuke.Security.Roles.RoleController rolController = new DotNetNuke.Security.Roles.RoleController();

            roles = rolController.GetRoles(PortalId);

            foreach (DotNetNuke.Security.Roles.RoleInfo rol in roles)
            {
                drow = dt.NewRow();
                drow["ROL_NAME"] = rol.RoleName;
                dt.Rows.Add(drow);
            }

            return dt;
        }

        #endregion

    }
}

