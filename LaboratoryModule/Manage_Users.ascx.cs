using DotNetNuke.Entities.Users;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Jar.Dnn.LaboratoryModule
{
    public partial class Manage_Users : LaboratoryModuleModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                System.Data.DataTable dtSchedules = Data.DataProvider.Instance().GetSchedules();
                System.Data.DataTable dtUsers = LoadUsers();

                CmbSchedules.DataSource = dtSchedules;
                CmbSchedules.DataTextField = "DESCRIPTION";
                CmbSchedules.DataValueField = "ID";
                CmbSchedules.DataBind();

                CmbDnnUsers.DataSource = dtUsers;
                CmbDnnUsers.DataTextField = "USERNAME";
                CmbDnnUsers.DataValueField = "USERID";
                CmbDnnUsers.DataBind();

                //LocalizateRadGrid(ref grdUsers);

                grdUsers.DataSource = dtUsers;
                grdUsers.DataBind();


                Grd_Schedules.DataSource = ConstructTable("SCHEDULES");
                Grd_Schedules.DataBind();

                GrdUsers_Roles.DataSource = ConstructTable("ROLES_USER");
                GrdUsers_Roles.DataBind();

                GrdBannedIp.DataSource = Data.DataProvider.Instance().GetBannedIP();
                GrdBannedIp.DataBind();
            }
        }


        #region Users

        protected void GrdUsers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            System.Data.DataTable dtUsers = new System.Data.DataTable();

            //LocalizateRadGrid(ref grdUsers);

            try
            {
                dtUsers = LoadUsers();
            }
            catch
            {
                dtUsers = ConstructTable("USERS");
            }

            grdUsers.DataSource = dtUsers;
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

                    CollapseAllRows(grdUsers, -1);
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;

                    Telerik.Web.UI.GridEditManager editMan = editedItem.EditManager;
                    Telerik.Web.UI.GridDropDownListColumnEditor editor = (Telerik.Web.UI.GridDropDownListColumnEditor)(editMan.GetColumnEditor("lstSchedules"));

                    editor.DataSource = Data.DataProvider.Instance().GetSchedules();
                    editor.DataTextField = "DESCRIPTION";
                    editor.DataValueField = "ID";
                    editor.DataBind();

                    editor.ComboBoxControl.Width = System.Web.UI.WebControls.Unit.Pixel(350);

                    System.Data.DataRowView dataItem = (System.Data.DataRowView)editedItem.DataItem;
                    if (dataItem != null)
                    {
                        editor.SelectedText = dataItem["SCHEDULE_DES"].ToString();
                        editor.SelectedValue = dataItem["SCHEDULE_ID"].ToString();
                    }
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
                    ((System.Web.UI.WebControls.LinkButton)dataItem["DelUsers"].FindControl("DeleteUser")).Attributes.Add("onClick", "onDeleteUser('" + dataItem.ItemIndex.ToString() + "');");
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
                int ScheduleId = 0;

                System.Data.DataTable dtUsers = new System.Data.DataTable();
                System.Data.DataTable dt = new System.Data.DataTable();
                int index = editedItem.ItemIndex;

                dtUsers = GetDataTable(grdUsers);

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
                        case "SCHEDULE_ID":
                            try
                            {
                                ScheduleId = int.Parse(item.Value.ToString());
                                if (Data.DataProvider.Instance().SetUserSchedule(user.UserID, ScheduleId) != 1)
                                {
                                    e.Canceled = true;
                                    grdUsers.MasterTableView.ClearEditItems();
                                    DisplayAlert("No se pudo actualizar el horario");
                                }
                            }
                            catch (Exception ex)
                            {
                                e.Canceled = true;

                                DisplayAlert(ex);
                            }
                            break;
                    }
                }

                try
                {
                    UserController.UpdateUser(PortalId, user);
                    grdUsers.MasterTableView.ClearEditItems();
                    DisplayAlert("Registro actualizado correctamente", "Registro Actualizado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    grdUsers.MasterTableView.ClearEditItems();

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
                int ScheduleId = 0;
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
                        case "SCHEDULE_ID":
                            ScheduleId = int.Parse(item.Value.ToString());
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
                        Data.DataProvider.Instance().SetUserSchedule(user.UserID, ScheduleId);

                        grdUsers.MasterTableView.ClearEditItems();
                        grdUsers.DataSource = LoadUsers();
                        grdUsers.DataBind();
                        DisplayAlert("Usuario creado correctamente", "Usuario Creado", "Info");
                    }
                    else
                    {
                        e.Canceled = true;
                        DisplayAlert("Error: " + addmsg);
                    }

                    //user = UserController.GetUserByName(user.Username);


                }
                catch (Exception ex)
                {
                    e.Canceled = true;

                    DisplayAlert(ex);
                }

            }
        }
        protected void GrdUsers_CancelCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
        }
        protected void GrdUsers_PreRender(object sender, EventArgs e)
        {
            //if (!string.IsNullOrEmpty(gridMessage))
            //{
            //    DisplayAlert(gridMessage, grdIPAddress, "ERRROR");
            //}
            //foreach (GridDataItem item in grdUsers.MasterTableView.Items)
            //{
            //    CollapseAllRows(grdUsers, -1);
            //}
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

                CollapseAllRows(grdUsers, e.Item.ItemIndex);
                e.Item.Expanded = !lastState;

                if (lastState)
                {
                    System.Data.DataTable dt = GetDataTable(grdUsers);
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
                            case "SendPassMail":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());
                                    item = grdUsers.Items[itemIndex];
                                    dtItem = (System.Data.DataRowView)item.DataItem;

                                    if (dtItem != null)
                                        dtrow = dtItem.Row;
                                    else
                                        dtrow = GetDataTable(grdUsers).Rows[itemIndex];

                                    int L_UserID = int.Parse(dtrow["USERID"].ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);
                                    UserController.ResetPasswordToken(usrDnn);
                                    DotNetNuke.Services.Mail.Mail.SendMail(usrDnn, DotNetNuke.Services.Mail.MessageType.PasswordReminder, PortalSettings);
                                    DisplayAlert("Enlace de restablecimiento de contrase&ntilde;a enviado correctamente.", "Restablecer Contraseña", "Info");
                                }
                                break;
                            case "LockedOut":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());
                                    item = grdUsers.Items[itemIndex];
                                    dtItem = (System.Data.DataRowView)item.DataItem;

                                    if (dtItem != null)
                                        dtrow = dtItem.Row;
                                    else
                                        dtrow = GetDataTable(grdUsers).Rows[itemIndex];

                                    int L_UserID = int.Parse(dtrow["USERID"].ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);

                                    UserController.UnLockUser(usrDnn);

                                    DisplayAlert("Usuario desbloqueado correctamente.", "Desbloquear Usuario", "Info");

                                    grdUsers.Rebind();
                                }
                                break;
                            case "Authorized":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    int itemIndex = int.Parse(e.CommandArgument.ToString());
                                    item = grdUsers.Items[itemIndex];
                                    dtItem = (System.Data.DataRowView)item.DataItem;

                                    if (dtItem != null)
                                        dtrow = dtItem.Row;
                                    else
                                        dtrow = GetDataTable(grdUsers).Rows[itemIndex];

                                    int L_UserID = int.Parse(dtrow["USERID"].ToString());
                                    UserInfo usrDnn = UserController.GetUserById(PortalId, L_UserID);
                                    string tmpmessage = "";

                                    if (usrDnn.Membership.Approved)
                                        tmpmessage = "desautorizado";
                                    else
                                        tmpmessage = "autorizado";

                                    usrDnn.Membership.Approved = !usrDnn.Membership.Approved;

                                    UserController.UpdateUser(PortalId, usrDnn);

                                    DisplayAlert("Usuario " + tmpmessage + " correctamente.", "Autorizar Usuario", "Info");

                                    grdUsers.Rebind();

                                }
                                break;
                            case "SetPassword":
                                if (!string.IsNullOrEmpty(e.CommandArgument.ToString()))
                                {
                                    string[] arguments = e.CommandArgument.ToString().Split('|');
                                    string password = arguments[1];

                                    int itemIndex = int.Parse(arguments[0]);
                                    item = grdUsers.Items[itemIndex];
                                    dtItem = (System.Data.DataRowView)item.DataItem;

                                    if (dtItem != null)
                                        dtrow = dtItem.Row;
                                    else
                                        dtrow = GetDataTable(grdUsers).Rows[itemIndex];

                                    int L_UserID = int.Parse(dtrow["USERID"].ToString());
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
                                    item = grdUsers.Items[itemIndex];
                                    dtItem = (System.Data.DataRowView)item.DataItem;

                                    if (dtItem != null)
                                        dtrow = dtItem.Row;
                                    else
                                        dtrow = GetDataTable(grdUsers).Rows[itemIndex];

                                    bool dnnDeleted = false;

                                    UserInfo user = UserController.GetUserById(PortalId, int.Parse(dtrow["USERID"].ToString()));

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

                                        grdUsers.DataSource = LoadUsers();
                                        grdUsers.DataBind();
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


        protected void Grd_Schedules_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            System.Data.DataTable dtShedules = new System.Data.DataTable();

            try
            {
                dtShedules = GetDataTable(Grd_Schedules);
            }
            catch
            {
                dtShedules = ConstructTable("SCHEDULES");
            }

            Grd_Schedules.DataSource = dtShedules;
        }
        protected void Grd_Schedules_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem && e.Item.IsInEditMode)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;

                    Telerik.Web.UI.GridEditManager editMan = editedItem.EditManager;
                    Telerik.Web.UI.GridDropDownListColumnEditor editor = (Telerik.Web.UI.GridDropDownListColumnEditor)(editMan.GetColumnEditor("lstDays"));

                    editor.DataSource = Data.DataProvider.Instance().GetSchedulesDays();
                    editor.DataTextField = "DAY";
                    editor.DataValueField = "ID";
                    editor.DataBind();

                    editor.ComboBoxControl.Width = System.Web.UI.WebControls.Unit.Pixel(150);

                    System.Data.DataRowView dataItem = (System.Data.DataRowView)editedItem.DataItem;
                    if (dataItem != null)
                    {
                        editor.SelectedText = dataItem["DAY"].ToString();
                        editor.SelectedValue = dataItem["DAY_ID"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    DisplayAlert(ex);
                }

            }
        }
        protected void Grd_Schedules_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.IsInEditMode && e.Item is Telerik.Web.UI.GridEditFormItem)
            {
                Telerik.Web.UI.GridEditFormItem editItem = e.Item as Telerik.Web.UI.GridEditFormItem;
                Telerik.Web.UI.RadDateTimePicker picker = editItem["TIME_START"].Controls[0] as Telerik.Web.UI.RadDateTimePicker;
                picker.SharedTimeView.TimeFormat = "HH:mm";
            }
        }
        protected void Grd_Schedules_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    System.Data.DataTable dtSchedules = new System.Data.DataTable();
                    System.Data.DataTable tmpSchedules = new System.Data.DataTable();
                    bool valid = true;

                    dtSchedules = GetDataTable(Grd_Schedules);
                    tmpSchedules = dtSchedules.Copy();

                    System.Data.DataTable dtDays = Data.DataProvider.Instance().GetSchedulesDays();

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    int DayID = 0;
                    string Day = string.Empty;
                    DateTime t_start = DateTime.Now;
                    DateTime t_end = DateTime.Now;

                    foreach (DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "DAY_ID":
                                DayID = int.Parse(item.Value.ToString());
                                break;
                            case "TIME_START":
                                t_start = DateTime.Parse(item.Value.ToString());
                                break;
                            case "TIME_END":
                                t_end = DateTime.Parse(item.Value.ToString());
                                break;
                        }
                    }

                    foreach (System.Data.DataRow tmprow in tmpSchedules.Rows)
                    {
                        if (DayID == int.Parse(tmprow["DAY_ID"].ToString()))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid)
                    {
                        e.Canceled = true;
                        DisplayAlert("El registro no se pudo crear, ya existe configurado un límite para el dia seleccionado.", "Alert", "Alert");
                    }
                    else
                    {
                        string desc = string.Empty;
                        int SchId = -1;
                        bool newReg = false;

                        if (CmbSchedules.SelectedItem != null)
                        {
                            desc = CmbSchedules.SelectedItem.Text;
                            SchId = int.Parse(CmbSchedules.SelectedItem.Value);

                            if (!string.IsNullOrEmpty(txtScheduleName.Text) && txtScheduleName.Text != desc)
                            {
                                Data.DataProvider.Instance().SetSchedule(txtScheduleName.Text, SchId);
                                newReg = true;
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(txtScheduleName.Text))
                            {
                                e.Canceled = true;
                                DisplayAlert("Si desea crear un nuevo horario, el campo DESCRIPCIÓN no puede estar vacío.", "Alerta", "Alert");
                                return;
                            }
                            else
                            {
                                SchId = Data.DataProvider.Instance().SetSchedule(txtScheduleName.Text);
                                newReg = true;
                            }
                        }

                        Data.DataProvider.Instance().SetScheduleLimits(SchId, DayID, t_start.TimeOfDay, t_end.TimeOfDay);

                        if (newReg)
                        {
                            CmbSchedules.DataSource = Data.DataProvider.Instance().GetSchedules();
                            CmbSchedules.DataTextField = "DESCRIPTION";
                            CmbSchedules.DataValueField = "ID";
                            CmbSchedules.DataBind();

                            CmbSchedules.SelectedValue = SchId.ToString();
                        }

                        Grd_Schedules.MasterTableView.ClearSelectedItems();
                        Grd_Schedules.MasterTableView.ClearEditItems();
                        Grd_Schedules.DataSource = Data.DataProvider.Instance().GetSchedules(SchId);
                        Grd_Schedules.Rebind();
                        DisplayAlert("Registro creado correctamente", "Registro Creado", "Info");
                    }
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    DisplayAlert(ex);
                }
            }
        }
        protected void Grd_Schedules_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int index = editedItem.ItemIndex;
                    System.Data.DataTable dtSchedules = new System.Data.DataTable();
                    System.Data.DataTable tmpSchedules = new System.Data.DataTable();
                    System.Data.DataTable dtDays = new System.Data.DataTable();
                    int i = 0;
                    bool isValid = true;

                    dtSchedules = GetDataTable(Grd_Schedules);
                    tmpSchedules = dtSchedules.Copy();


                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    int SchId = int.Parse(dtSchedules.Rows[index]["SCH_ID"].ToString());
                    int DayId = int.Parse(dtSchedules.Rows[index]["DAY_ID"].ToString());
                    string Day = dtSchedules.Rows[index]["DAY"].ToString();
                    DateTime t_start = DateTime.Parse(dtSchedules.Rows[index]["TIME_START"].ToString());
                    DateTime t_end = DateTime.Parse(dtSchedules.Rows[index]["TIME_END"].ToString());

                    foreach (DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "DAY_ID":
                                DayId = int.Parse(item.Value.ToString());
                                break;
                            case "TIME_START":
                                t_start = DateTime.Parse(item.Value.ToString());
                                break;
                            case "TIME_END":
                                t_end = DateTime.Parse(item.Value.ToString());
                                break;
                        }
                    }

                    foreach (System.Data.DataRow tmprow in tmpSchedules.Rows)
                    {
                        if (i != index)
                        {
                            if (DayId == int.Parse(tmprow["DAY_ID"].ToString()))
                            {
                                isValid = false;
                                break;
                            }
                        }

                        i++;
                    }

                    if (!isValid)
                    {
                        e.Canceled = true;
                        DisplayAlert("El registro no se pudo actualizar, ya existe configurado un horario para el dia seleccionado.", "Alerta", "Alert");
                    }
                    else
                    {
                        dtDays = Data.DataProvider.Instance().GetSchedulesDays();

                        foreach (System.Data.DataRow newRow in dtDays.Select(string.Format("ID = {0}", DayId)))
                        {
                            Day = newRow["DAY"].ToString();
                        }

                        dtSchedules.Rows[index]["DAY_ID"] = DayId;
                        dtSchedules.Rows[index]["DAY"] = Day;
                        dtSchedules.Rows[index]["TIME_START"] = t_start.ToShortTimeString();
                        dtSchedules.Rows[index]["TIME_END"] = t_end.ToShortTimeString();

                        Grd_Schedules.MasterTableView.ClearEditItems();
                        Grd_Schedules.DataSource = dtSchedules;
                        Grd_Schedules.Rebind();
                        DisplayAlert("Registro actualizado correctamente", "Registro Actualizado", "Info");
                    }
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    DisplayAlert(ex);
                }
            }
        }
        protected void Grd_Schedules_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int index = editedItem.ItemIndex;
                    int SchId = 0;
                    int DayId = 0;
                    bool DeletedAll = false;
                    System.Data.DataTable dtSchedules = new System.Data.DataTable();

                    dtSchedules = GetDataTable(Grd_Schedules);

                    SchId = int.Parse(dtSchedules.Rows[index]["SCH_ID"].ToString());
                    DayId = int.Parse(dtSchedules.Rows[index]["DAY_ID"].ToString());

                    if (dtSchedules.Rows.Count == 1)
                    {
                        Data.DataProvider.Instance().DeleteSchedule(SchId);
                        dtSchedules = ConstructTable("SCHEDULES");
                        DeletedAll = true;
                    }
                    else
                    {
                        Data.DataProvider.Instance().DeleteScheduleLimits(SchId, DayId);
                        dtSchedules = Data.DataProvider.Instance().GetSchedules(SchId);
                    }

                    if (DeletedAll)
                    {
                        CmbSchedules.ClearSelection();
                        CmbSchedules.DataSource = Data.DataProvider.Instance().GetSchedules();
                        CmbSchedules.DataTextField = "DESCRIPTION";
                        CmbSchedules.DataValueField = "ID";
                        CmbSchedules.DataBind();
                        txtScheduleName.Text = string.Empty;
                    }

                    Grd_Schedules.DataSource = dtSchedules;
                    Grd_Schedules.DataBind();
                    DisplayAlert("Límite Eliminado Correctamente. ", "Límite Eliminado", "Info");
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    DisplayAlert(ex);
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
                }
                catch (Exception ex)
                {
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, ex.Message + ex.StackTrace, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
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

        protected void GrdBannedIp_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            System.Data.DataTable dtBannedIp = new System.Data.DataTable();

            try
            {
                dtBannedIp = GetDataTable(GrdBannedIp);
            }
            catch
            {
                dtBannedIp = ConstructTable("BANNED_IP");
            }

            GrdBannedIp.DataSource = dtBannedIp;
        }
        protected void GrdBannedIp_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem && e.Item.IsInEditMode)
            {
                try
                {

                }
                catch (Exception ex)
                {
                    DotNetNuke.UI.Skins.Skin.AddModuleMessage(this, ex.Message + ex.StackTrace, DotNetNuke.UI.Skins.Controls.ModuleMessage.ModuleMessageType.RedError);
                }

            }
        }
        protected void GrdBannedIp_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    string Ip = string.Empty;
                    string Description = string.Empty;
                    bool exist = false;

                    System.Data.DataTable dtBannedIp = new System.Data.DataTable();

                    dtBannedIp = GetDataTable(GrdBannedIp);

                    Hashtable newValues = new Hashtable();
                    e.Item.OwnerTableView.ExtractValuesFromItem(newValues, editedItem);

                    foreach (DictionaryEntry item in newValues)
                    {
                        switch (item.Key.ToString())
                        {
                            case "IP":
                                Ip = item.Value.ToString();
                                break;
                            case "DESCRIPTION":
                                Description = item.Value.ToString();
                                break;
                        }
                    }

                    foreach (System.Data.DataRow drow in dtBannedIp.Select(string.Format("IP = '{0}'", Ip)))
                    {
                        exist = true;
                    }

                    if (exist)
                    {
                        e.Canceled = true;
                        DisplayAlert("Error: La IP que intenta registrar ya se encuentra en el sistema.", "Alerta", "Alert");
                    }
                    else
                    {
                        Data.DataProvider.Instance().SetBannedIP(Ip, UserInfo.Username, Description);
                        GrdBannedIp.MasterTableView.ClearEditItems();
                        GrdBannedIp.DataSource = Data.DataProvider.Instance().GetBannedIP();
                        GrdBannedIp.Rebind();
                        DisplayAlert("Ip agregada correctamente", "Agregar IP", "Info");
                    }
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    DisplayAlert(ex);
                }
            }
        }
        protected void GrdBannedIp_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridEditableItem)
            {
                try
                {
                    Telerik.Web.UI.GridEditableItem editedItem = e.Item as Telerik.Web.UI.GridEditableItem;
                    int index = editedItem.ItemIndex;
                    string Ip = string.Empty;

                    System.Data.DataTable dtBannedIp = new System.Data.DataTable();
                    DotNetNuke.Security.Roles.RoleController rolController = new DotNetNuke.Security.Roles.RoleController();

                    dtBannedIp = GetDataTable(GrdBannedIp);

                    Ip = dtBannedIp.Rows[index]["IP"].ToString();

                    if (Data.DataProvider.Instance().DeleteBannedIP(Ip) == 1)
                    {
                        GrdBannedIp.MasterTableView.ClearEditItems();
                        GrdBannedIp.DataSource = Data.DataProvider.Instance().GetBannedIP();
                        GrdBannedIp.Rebind();
                        DisplayAlert("IP eliminada correctamente", "Eliminar IP", "Info");
                    }
                    else
                    {
                        e.Canceled = true;
                        DisplayAlert("La IP no se pudo eliminar.", "Alerta", "Alert");
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
        protected void CmbSchedules_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string ScheduleId = e.Value;
            try
            {
                Grd_Schedules.DataSource = Data.DataProvider.Instance().GetSchedules(int.Parse(ScheduleId));
                Grd_Schedules.DataBind();
                txtScheduleName.Text = e.Text;
                lblScheduleName.Visible = true;
                txtScheduleName.Visible = true;
                Session["dtSchElim"] = null;
            }
            catch (Exception ex)
            {
                DisplayAlert(ex);

                Grd_Schedules.DataSource = ConstructTable("SCHEDULES");
                Grd_Schedules.DataBind();
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
            System.Data.DataTable dtShceduleUsers = new System.Data.DataTable();
            System.Data.DataRow drow;
            string horario = "No Especificado";
            string interval = string.Empty;
            string scheduleId = string.Empty;
            System.Collections.ArrayList lstDnnUsers;

            dt = ConstructTable("USERS");

            try
            {
                lstDnnUsers = UserController.GetUsers(PortalId);
                dtShceduleUsers = (Data.DataProvider.Instance().GetUsersSchedules());

                foreach (UserInfo usrInfo in lstDnnUsers)
                {
                    horario = "No Especificado";
                    interval = string.Empty;
                    scheduleId = string.Empty;

                    if (dtShceduleUsers.Rows.Count > 0)
                    {
                        foreach (System.Data.DataRow drwsch in dtShceduleUsers.Rows)
                        {
                            if (usrInfo.UserID == int.Parse(drwsch["USER_ID"].ToString()))
                            {
                                horario = drwsch["DESCRIPTION"].ToString();
                                scheduleId = drwsch["SCHEDULE_ID"].ToString();
                                break;
                            }
                        }
                    }

                    drow = dt.NewRow();

                    drow["USERID"] = usrInfo.UserID;
                    drow["USERNAME"] = usrInfo.Username;
                    drow["DISPLAYNAME"] = usrInfo.DisplayName;
                    drow["NAME"] = usrInfo.FirstName;
                    drow["LASTNAME"] = usrInfo.LastName;
                    drow["EMAIL"] = usrInfo.Email;
                    drow["PASSWORD"] = "";
                    drow["SCHEDULE_DES"] = horario;
                    drow["SCHEDULE_ID"] = scheduleId;
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