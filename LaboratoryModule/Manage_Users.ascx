<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Manage_Users.ascx.cs" Inherits="Jar.Dnn.LaboratoryModule.Manage_Users" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/Jar.Dnn/LaboratoryModule/Resources/Css/Laboratory.css" />

<telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
    <script type="text/javascript">
        function showDialogNotification(message, title, width, height, type) {
            var oWnd = radalert(message, title);
            var content = oWnd.get_contentElement().getElementsByTagName("DIV");
            oWnd.set_title(title);
            oWnd.width = width;
            oWnd.height = height;
            for (var i in content) {
                if (content[i].className == "rwDialogPopup radalert") {
                    if (type == "Info") {
                        content[i].className += " radinformation";
                    } else if (type == "Error") {
                        content[i].className += " raderror";
                    }
                }
            }
        }

        function onTabSelected(sender, args) {
            ReloadTime();
        }

        /// Funciones de los comandos del Grid

        function onDeleteUser(index) {
            RowIndex = index;
            radconfirm('¿Seguro desea eliminar el usuario completamente?', callBackDelUsrFn, 250, 150, '', 'Eliminar Usuario');
        }

        function callBackDelUsrFn(confirmed) {
            if (confirmed) {
                $find('<%= grdUsers.ClientID%>').get_masterTableView().fireCommand("DeleteUsr", RowIndex);
            }
        }

        function onSendMailPass(index) {
            RowIndex = index;
            radconfirm('¿Enviar el enlace de restablecimiento de contrase&ntilde;a ?', callBackSendMailPassFn, 250, 150, '', 'Enviar Restablecimiento Contrase&ntilde;a');
        }

        function callBackSendMailPassFn(confirmed) {
            if (confirmed) {
                $find('<%= grdUsers.ClientID%>').get_masterTableView().fireCommand("SendPasswordMail", RowIndex);
            }
        }

        function onAuthorizedUser(index) {
            RowIndex = index;
            radconfirm('¿Autorizar Usuario?', callBackAuthorizedUserFn, 250, 150, '', 'Usuario Desautorizado');
        }

        function callBackAuthorizedUserFn(confirmed) {
            if (confirmed) {
                $find('<%= grdUsers.ClientID%>').get_masterTableView().fireCommand("Authorized", RowIndex);
            }
        }

        function onNotAuthorizedUser(index) {
            RowIndex = index;
            radconfirm('¿Desautorizar Usuario?', callBackNotAuthorizedUserFn, 250, 150, '', 'Usuario Autorizado');
        }

        function callBackNotAuthorizedUserFn(confirmed) {
            if (confirmed) {
                $find('<%= grdUsers.ClientID%>').get_masterTableView().fireCommand("Authorized", RowIndex);
            }
        }

        function onLockedOut(index) {
            RowIndex = index;
            radconfirm('¿Desbloquear Usuario?', callBackLockedOutFn, 350, 250, '', 'Usuario Bloqueado');
        }

        function callBackLockedOutFn(confirmed) {
            if (confirmed) {
                $find('<%= grdUsers.ClientID%>').get_masterTableView().fireCommand("LockedOut", RowIndex);
            }
        }

        function onSetPassword(index) {
            RowIndex = index;
            radprompt('Nueva Contrase&ntilde;a?', callBackSetPasswordFn, 350, 230, null, 'Cambiar Contrase&ntilde;a', ''); return false;
            radconfirm('¿Enviar el enlace de restablecimiento de contrase&ntilde;a ?', callBackSetPasswordFn, 350, 250, '', 'Enviar Restablecimiento Contrase&ntilde;a');
        }

        function callBackSetPasswordFn(password) {
            if (typeof (password) != 'undefined' && password != null) {
                if (password.includes("|")) {
                    radalert("Error: No se permite el pipe &apos;|&apos; dentro de la contrase&ntilde;a", 350, 250, "Cambiar Contrase&ntilde;a");
                } else {
                    var argument = RowIndex + "|" + password;
                    $find('<%= grdUsers.ClientID%>').get_masterTableView().fireCommand("SetPassword", argument);
                }
            } else {
                radalert("Error: No se ingres&oacute; ninguna contrase&ntilde;a, por lo tanto no se ejecutar&aacute; ninguna acci&oacute;n.", 350, 250, "Cambiar Contrase&ntilde;a");
            }
        }

    /*
    
    
    
    onDeleteUser
    */

    /// Fin Funciones de comandos del Grid

    </script>
</telerik:RadScriptBlock>

<telerik:RadTabStrip ID="tabs" runat="server" MultiPageID="pages" SelectedIndex="0" OnClientTabSelected="onTabSelected">
    <Tabs>
        <telerik:RadTab Text="Usuarios" PageViewID="users" />
        <telerik:RadTab Text="Roles de Usuarios" PageViewID="userRolesPage" />
        <telerik:RadTab Text="Bannear IP" PageViewID="bannedIPs" />
        <telerik:RadTab Text="Horarios" PageViewID="schedules" />
    </Tabs>
</telerik:RadTabStrip>

<telerik:RadMultiPage ID="pages" runat="server" SelectedIndex="0">
    <telerik:RadPageView ID="users" runat="server">
        <div class="gridLayout">
            <telerik:RadGrid ID="grdUsers" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowPaging="True" PageSize="10" Skin="MetroTouch"
                OnItemDataBound="GrdUsers_ItemDataBound" OnItemUpdated="GrdUsers_ItemUpdated" OnUpdateCommand="GrdUsers_UpdateCommand" OnPreRender="GrdUsers_PreRender"
                OnItemCommand="GrdUsers_ItemCommand" OnNeedDataSource="GrdUsers_NeedDataSource" OnInsertCommand="GrdUsers_InsertCommand" BorderColor="#777" BorderStyle="Solid" AllowFilteringByColumn="true"
                HeaderStyle-BackColor="#0075bc" HeaderStyle-Font-Bold="true" HeaderStyle-ForeColor="#ffffff" BackColor="#BDD6EE">

                <HeaderStyle BackColor="#0075bc" ForeColor="#FFFFFF" />
                <PagerStyle Mode="NextPrevAndNumeric" />
                <GroupingSettings CaseSensitive="false" />

                <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top" DataKeyNames="USERNAME" ClientDataKeyNames="USERNAME" TableLayout="Fixed"
                    NoDetailRecordsText="No hay registros secundarios para mostrar." NoMasterRecordsText="No hay registros para mostrar.">
                    <CommandItemSettings RefreshText="Refrescar" AddNewRecordText="Agregar Usuario" />
                    <RowIndicatorColumn Visible="False" />
                    <ExpandCollapseColumn Created="True" />
                    <Columns>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" EditText="Editar" HeaderStyle-Width="50" ItemStyle-CssClass="CustomCommands" />
                        <telerik:GridTemplateColumn HeaderText="" UniqueName="Actions" AllowFiltering="false" HeaderStyle-Width="140"  ItemStyle-CssClass="CustomCommands" >
                            <ItemTemplate>
                                <asp:LinkButton ID="SendPassMail" CommandName="SendPasswordMail" runat="server" ToolTip="Enviar Enlace Contrase&ntilde;a" CssClass="RadAButton rbLink rbIcon rbEMail" />
                                <asp:LinkButton ID="LockedOut" CommandName="LockedOut" Text="" runat="server" />
                                <asp:LinkButton ID="Authorized" CommandName="Authorized" Text="" runat="server" />
                                <asp:LinkButton ID="SetPassword" CommandName="SetPassword" Text="" runat="server" ToolTip="Cambiar Contrase&ntilde;a" CssClass="RadAButton rbLink rbIcon rbSetPass" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="USERID" UniqueName="USERID" Display="false" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="USERNAME" HeaderText="Usuario" UniqueName="USERNAME" HeaderStyle-Width="125"
                            ShowFilterIcon="false" AutoPostBackOnFilter="true" FilterListOptions="VaryByDataTypeAllowCustom" FilterControlWidth="100px">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="DISPLAYNAME" HeaderText="Nombre a Mostrar" UniqueName="DISPLAYNAME" HeaderStyle-Width="200"
                            ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" FilterControlWidth="190px">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NAME" HeaderText="Nombre" UniqueName="NAME" Display="false">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="LASTNAME" HeaderText="Apellido" UniqueName="LASTNAME" Display="false">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="EMAIL" HeaderText="Correo" UniqueName="EMAIL" HeaderStyle-Width="225" ShowFilterIcon="false"
                            AutoPostBackOnFilter="true" FilterListOptions="VaryByDataTypeAllowCustom" FilterControlWidth="200px">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="SCHEDULE_ID" HeaderText="Horario" UniqueName="SCHEDULE_ID" ReadOnly="true" Display="false" />
                        <telerik:GridDropDownColumn DataField="SCHEDULE_ID" UniqueName="lstSchedules" HeaderText="Horario" Visible="false" FilterListOptions="AllowAllFilters" />
                        <telerik:GridBoundColumn DataField="SCHEDULE_DES" HeaderText="Horario" UniqueName="SCHEDULE_DES" ReadOnly="true" HeaderStyle-Width="150"
                            ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" FilterControlWidth="140px" />
                        <telerik:GridBoundColumn DataField="AUTHORIZED" HeaderText="Autorizado" UniqueName="AUTHORIZED" ReadOnly="true" HeaderStyle-Width="90"
                            ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" FilterControlWidth="70px"/>
                        <telerik:GridBoundColumn DataField="LOCKED_OUT" HeaderText="" UniqueName="LOCKED_OUT" ReadOnly="true" Display="false" />
                        <telerik:GridBoundColumn DataField="IS_ONLINE" HeaderText="Online" UniqueName="IS_ONLINE" ReadOnly="true" Display="false" />
                        <telerik:GridBoundColumn DataField="PASSWORD" HeaderText="Contraseña" UniqueName="PASSWORD" Display="false">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn HeaderText="" UniqueName="DelUsers" AllowFiltering="false" HeaderStyle-Width="50"  ItemStyle-CssClass="CustomCommands" >
                            <ItemTemplate>
                                <asp:LinkButton ID="DeleteUser" CommandName="DeleteUsr" Text="" CssClass="RadAButton rbLink rbIcon rbDeleteUsr" runat="server" ToolTip="Borrar Usuario Completamente" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NestedViewTemplate>
                        <div class="UserDetails">
                            <div class="dLabel">
                                <span>UserName:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("USERNAME") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Nombre a Mostrar:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("DISPLAYNAME") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Horario:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("SCHEDULE_DES") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Nombre:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("NAME") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Apellido:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("LASTNAME") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Email:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("EMAIL") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Autorizado:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("AUTHORIZED") %></span>
                            </div>
                            <div class="dLabel">
                                <span>Bloqueado:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("LOCKED_OUT") %>&nbsp;</span>
                            </div>
                            <div class="dLabel">
                                <span>Online:</span>
                            </div>
                            <div class="dDescription">
                                <span><%# Eval("IS_ONLINE") %>&nbsp;</span>
                            </div>
                            <div class="dLabel">
                                <span>Lista de Roles:</span>
                            </div>
                            <div class="dDescription">
                                <telerik:RadGrid ID="GrdDetail_Roles" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" PageSize="50" CssClass="GrdSchedules" Width="600px">
                                    <MasterTableView DataKeyNames="ROL_ID" NoMasterRecordsText="No hay registros para mostrar." AutoGenerateColumns="False" CommandItemDisplay="None"
                                        NoDetailRecordsText="No hay registros secundarios para mostrar.">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="ROL_NAME" UniqueName="ROL_NAME" HeaderText="Rol" HeaderStyle-Width="420px" ReadOnly="true"/>
                                            <telerik:GridDateTimeColumn DataField="EFFECTIVE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                                                HeaderText="Fecha Creación" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="190px"  HtmlEncode="false"/>
                                            <telerik:GridDateTimeColumn DataField="EXPIRE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                                                HeaderText="Fecha Expiración" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="190px" EditDataFormatString="MM/dd/yyyy" />
                                        </Columns>
                                    </MasterTableView>
                                    <StatusBarSettings LoadingText="Cargando..." ReadyText="Listo" />
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </NestedViewTemplate>
                </MasterTableView>
                <StatusBarSettings LoadingText="Cargando..." ReadyText="Listo" />
            </telerik:RadGrid>
        </div>
    </telerik:RadPageView>
    <telerik:RadPageView ID="userRolesPage" runat="server">
        <div class="dnnForm dnnEdit dnnClear">
            <div class="dnnFormItemList">
                <div class="dnnLabel_">
                    <telerik:RadLabel ID="lblUsers" runat="server" Text="Usuarios: " />
                </div>
                <div class="dnnInput_">
                    <telerik:RadComboBox Width="50%" DataValueField="USERID" EmptyMessage="Seleccione un Usuario" OnSelectedIndexChanged="CmbDnnUsers_SelectedIndexChanged"
                        Filter="Contains" DataTextField="USERNAME" RenderMode="Lightweight" ID="CmbDnnUsers" CssClass="cmbDetails" runat="server" Font-Size="Small" AutoPostBack="true">
                        <HeaderTemplate>
                            <table style="width: 100%; text-align: left; font-size: small">
                                <tr>
                                    <td width="30%">USUARIO</td>
                                    <td width="70%">NOMBRE A MOSTRAR</td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%; text-align: left; font-size: small">
                                <tr>
                                    <td width="30%"><%# DataBinder.Eval(Container.DataItem, "USERNAME") %></td>
                                    <td width="70%"><%# DataBinder.Eval(Container.DataItem, "DISPLAYNAME") %></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
                </div>
            </div>
        </div>
        <div class="gridLayout">
            <telerik:RadGrid ID="GrdUsers_Roles" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowPaging="True" AllowAutomaticUpdates="True" AllowAutomaticInserts="False"
                AllowAutomaticDeletes="true" OnNeedDataSource="GrdUsers_Roles_NeedDataSource" OnInsertCommand="GrdUsers_Roles_InsertCommand" OnItemDataBound="GrdUsers_Roles_ItemDataBound"
                OnDeleteCommand="GrdUsers_Roles_DeleteCommand" PageSize="50" CssClass="GrdSchedules" Width="600px">

                <MasterTableView DataKeyNames="ROL_ID" NoMasterRecordsText="No hay registros para mostrar." AutoGenerateColumns="False" CommandItemDisplay="Top"
                    NoDetailRecordsText="No hay registros secundarios para mostrar.">
                    <CommandItemSettings AddNewRecordText="Agregar Rol" RefreshText="Refrescar" />
                </MasterTableView>

                <PagerStyle Mode="NextPrevAndNumeric" />
                <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top"
                    NoDetailRecordsText="No hay registros secundarios para mostrar." NoMasterRecordsText="No hay registros para mostrar.">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ROL_ID" UniqueName="ROL_ID" ReadOnly="true" Display="false"/>
                        <telerik:GridBoundColumn DataField="ROL_NAME" UniqueName="ROL_NAME" HeaderText="Rol" HeaderStyle-Width="420px" ReadOnly="true"/>
                        <telerik:GridDropDownColumn DataField="ROL_NAME" UniqueName="lstRoles" HeaderText="Rol" Visible="false" FilterListOptions="AllowAllFilters" />
                        <telerik:GridDateTimeColumn DataField="EFFECTIVE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                            HeaderText="Fecha Creación" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="190px"  HtmlEncode="false"/>
                        <telerik:GridDateTimeColumn DataField="EXPIRE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                            HeaderText="Fecha Expiración" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="190px" EditDataFormatString="MM/dd/yyyy" />
                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" HeaderStyle-Width="50px" />
                    </Columns>
                </MasterTableView>
                <StatusBarSettings LoadingText="Cargando..." ReadyText="Listo" />
            </telerik:RadGrid>
        </div>
    </telerik:RadPageView>
    <telerik:RadPageView ID="bannedIPs" runat="server">
        <div class="gridLayout">
            <div class="gridLayout">
            <telerik:RadGrid ID="GrdBannedIp" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowPaging="True" AllowAutomaticUpdates="True" AllowAutomaticInserts="False"
                AllowAutomaticDeletes="true" OnNeedDataSource="GrdBannedIp_NeedDataSource" OnInsertCommand="GrdBannedIp_InsertCommand" OnItemDataBound="GrdBannedIp_ItemDataBound"
                OnDeleteCommand="GrdBannedIp_DeleteCommand" PageSize="50" CssClass="GrdSchedules">

                <MasterTableView DataKeyNames="IP" NoMasterRecordsText="No hay registros para mostrar." AutoGenerateColumns="False" CommandItemDisplay="Top"
                    NoDetailRecordsText="No hay registros secundarios para mostrar.">
                    <CommandItemSettings AddNewRecordText="Agregar IP" RefreshText="Refrescar" />
                </MasterTableView>

                <PagerStyle Mode="NextPrevAndNumeric" />
                <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top"
                    NoDetailRecordsText="No hay registros secundarios para mostrar." NoMasterRecordsText="No hay registros para mostrar.">
                    <Columns>
                        <telerik:GridBoundColumn DataField="IP" UniqueName="IP" HeaderText="IP" HeaderStyle-Width="150px" >
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridDateTimeColumn DataField="DATE_LOCKED" DataType="System.DateTime" PickerType="DatePicker"
                            HeaderText="Fecha Creación" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="190px"  HtmlEncode="false"/>
                        <telerik:GridBoundColumn DataField="USERNAME" UniqueName="USERNAME" HeaderText="Agregada Por" HeaderStyle-Width="150px" ReadOnly="true"/>
                        <telerik:GridBoundColumn DataField="DESCRIPTION" UniqueName="DESCRIPTION" HeaderText="Descripción" FilterListOptions="AllowAllFilters" 
                            HeaderStyle-Width="350px" >
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridBoundColumn>
                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" HeaderStyle-Width="50px" />
                    </Columns>
                </MasterTableView>
                <StatusBarSettings LoadingText="Cargando..." ReadyText="Listo" />
            </telerik:RadGrid>
        </div>
        </div>
    </telerik:RadPageView>
    <telerik:RadPageView ID="schedules" runat="server">
        <div class="dnnForm dnnEdit dnnClear">
            <div class="dnnFormItemList">
                <p class="alert alert-info" role="alert">
                    <span aria-hidden="true" class="glyphicon glyphicon-exclamation-sign">
                        <span style="word-spacing:  -11px !important;">&nbsp;<b>Notas: </b><br /><br />
                            - Si desea agregar un nuevo horario solo tiene que colocar el nombre en el campo Descripción y luego agregar un límite y el mismo se creará automaticamente.<br /><br />
                            - Al borrar los límites si el horario queda sín límite alguno se borrará automaticamente.
                        </span>
                    </span>
                </p>
            </div>
            <div class="dnnFormItemList">
                <div class="dnnLabel_">
                    <telerik:RadLabel ID="lblCmbSchedules" runat="server" Text="Horarios: " />
                </div>
                <div class="dnnInput_">
                    <telerik:RadComboBox Width="30%" DataValueField="ID" EmptyMessage="Seleccione un Horario" OnSelectedIndexChanged="CmbSchedules_SelectedIndexChanged"
                        Filter="Contains" DataTextField="DESCRIPTION" RenderMode="Lightweight" ID="CmbSchedules" CssClass="cmbDetails" runat="server" Font-Size="Small" 
                        AutoPostBack="true">
                        <HeaderTemplate>
                            <table style="width: 100%; text-align: left; font-size: small">
                                <tr>
                                    <td width="100%">HORARIOS</td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table style="width: 100%; text-align: left; font-size: small">
                                <tr>
                                    <td width="100%"><%# DataBinder.Eval(Container.DataItem, "DESCRIPTION") %></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </telerik:RadComboBox>
                </div>
            </div>
            <div class="dnnFormItemList">
                <div class="dnnLabel_">
                    <telerik:RadLabel ID="lblScheduleName" runat="server" Text="Descripción: "/>
                </div>
                <div class="dnnInput_">
                    <telerik:RadTextBox ID="txtScheduleName" runat="server" />
                </div>
            </div>
        </div>
        <div class="gridLayout">
            <telerik:RadGrid ID="Grd_Schedules" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowPaging="True" AllowAutomaticUpdates="True" AllowAutomaticInserts="False"
                AllowAutomaticDeletes="true" OnNeedDataSource="Grd_Schedules_NeedDataSource" OnInsertCommand="Grd_Schedules_InsertCommand" OnItemDataBound="Grd_Schedules_ItemDataBound"
                OnUpdateCommand="Grd_Schedules_UpdateCommand" OnItemCreated="Grd_Schedules_ItemCreated" OnDeleteCommand="Grd_Schedules_DeleteCommand" PageSize="50" CssClass="GrdSchedules">

                <MasterTableView DataKeyNames="SCH_ID" NoMasterRecordsText="No hay registros para mostrar." AutoGenerateColumns="False" CommandItemDisplay="Top"
                    NoDetailRecordsText="No hay registros secundarios para mostrar.">
                    <CommandItemSettings AddNewRecordText="Agregar Límite" RefreshText="Refrescar" />
                </MasterTableView>

                <PagerStyle Mode="NextPrevAndNumeric" />
                <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top"
                    NoDetailRecordsText="No hay registros secundarios para mostrar." NoMasterRecordsText="No hay registros para mostrar.">
                    <Columns>
                        <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" />
                        <telerik:GridBoundColumn DataField="SCH_ID" UniqueName="SCH_ID" Display="false" InsertVisiblityMode="AlwaysHidden" ReadOnly="True" />
                        <telerik:GridBoundColumn DataField="DAY_ID" UniqueName="DAY_ID" Display="false" InsertVisiblityMode="AlwaysHidden" ReadOnly="True" />
                        <telerik:GridDropDownColumn DataField="DAY_ID" UniqueName="lstDays" HeaderText="Día" Visible="false" FilterListOptions="AllowAllFilters" />
                        <telerik:GridBoundColumn DataField="DAY" UniqueName="DAY" HeaderText="Día" ReadOnly="true" />
                        <telerik:GridDateTimeColumn DataField="TIME_START" DataType="System.DateTime" PickerType="TimePicker"
                            HeaderText="Hora Inicio" DataFormatString="{0:HH\:mm}" HeaderStyle-Width="160px" EditDataFormatString="HH\:mm">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>
                        <telerik:GridDateTimeColumn DataField="TIME_END" DataType="System.DateTime" PickerType="TimePicker"
                            HeaderText="Hora Fin" DataFormatString="{0:HH\:mm}" HeaderStyle-Width="160px" EditDataFormatString="HH\:mm">
                            <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                            </ColumnValidationSettings>
                        </telerik:GridDateTimeColumn>

                        <telerik:GridButtonColumn CommandName="Delete" Text="Delete" />
                    </Columns>
                </MasterTableView>
                <StatusBarSettings LoadingText="Cargando..." ReadyText="Listo" />
            </telerik:RadGrid>
        </div>
    </telerik:RadPageView>
</telerik:RadMultiPage>


<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="grdUsers">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="grdUsers" LoadingPanelID="loadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="CmbSchedules">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="Grd_Schedules" LoadingPanelID="loadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="lblScheduleName" />
                <telerik:AjaxUpdatedControl ControlID="txtScheduleName" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="Grd_Schedules">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="Grd_Schedules" LoadingPanelID="loadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="CmbSchedules" />
                <telerik:AjaxUpdatedControl ControlID="txtScheduleName" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="CmbDnnUsers">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdUsers_Roles" LoadingPanelID="loadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="grdUsers" LoadingPanelID="loadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdUsers_Roles">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdUsers_Roles" LoadingPanelID="loadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="grdUsers" LoadingPanelID="loadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>

<telerik:RadAjaxLoadingPanel ID="loadingPanel" runat="server" Skin="MetroTouch" />

<telerik:RadWindowManager runat="server" ID="winManager" Width="350" ReloadOnShow="true" DestroyOnClose="true">
    <Localization OK="Ok" Cancel="Cancelar" Yes="Sí" />
    <ConfirmTemplate>
        <div class="rwDialogPopup radconfirm">
            <div class="rwDialogText">
                {1}
            </div>
            <div>
                <a onclick="$find('{0}').close(true);" class="rwPopupButton confirmButton" href="javascript:void(0);">
                    <span class="rwOuterSpan">
                        <span class="rwInnerSpan">##LOC[Yes]##</span>
                    </span>
                </a>
                <a onclick="$find('{0}').close(false);" class="rwPopupButton cancelButton" href="javascript:void(0);">
                    <span class="rwOuterSpan">
                        <span class="rwInnerSpan">##LOC[No]##</span>
                    </span>
                </a>
            </div>
        </div>
    </ConfirmTemplate>
    <PromptTemplate>
        <div class="rwDialog rwPromptDialog">
            <div class="rwDialogContent">
                <div class="rwDialogMessage">{1}</div>
                <div class="rwPromptInputContainer">
                    <input title="Enter Value" type="password" class="rwPromptInput radPreventDecorate" value="{2}" />
                </div>
            </div>
            <div class="rwDialogButtons">
                <a onclick="$find('{0}').close(this.parentNode.parentNode.getElementsByTagName('input')[0].value); return false;" class="rwPopupButton confirmButton" href="javascript:void(0);">
                    <span class="rwOuterSpan">
                        <span class="rwInnerSpan">##LOC[OK]##</span>
                    </span>
                </a>
                <a onclick="$find('{0}').close(null); return false;" class="rwPopupButton cancelButton" href="javascript:void(0);">
                    <span class="rwOuterSpan">
                        <span class="rwInnerSpan">##LOC[No]##</span>
                    </span>
                </a>
            </div>
        </div>
    </PromptTemplate>
</telerik:RadWindowManager>
