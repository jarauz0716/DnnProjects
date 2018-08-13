<%@ Control Language="C#" AutoEventWireup="false" Inherits="Jar.Dnn.LaboratoryModule.Laboratory" CodeFile="Laboratory.ascx.cs" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<dnn:DnnCssInclude runat="server" FilePath="~/DesktopModules/Jar.Dnn/LaboratoryModule/Resources/Css/Laboratory.css" />

<telerik:RadCodeBlock runat="server">
    <script type="text/javascript">
        function onRequestStart(sender, args) {
            try {
                if (args.get_eventTarget().indexOf("BtnExport") >= 0) {
                    //ReloadTime();
                    args.set_enableAjax(false);
                }
            }
            catch (e) {
                console.log(e);
            }
        }
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

        function OnClientClicking(sender, args) {
            var callBackFunction = Function.createDelegate(sender, function (argument) {
                if (argument) {
                    this.click();
                }
            });
            var text = "Al procesar el exámen el mismo no podrá ser modificado, ¿Desea Continuar?";
            radconfirm(text, callBackFunction, 350, 200, null, "Procesar Exámen");
            args.set_cancel(true);
        }

        function onTabSelected(sender, args) {
            //ReloadTime();
        }

        /// Funciones de los comandos del Grid

        function onDeleteUser(index) {
            RowIndex = index;
            radconfirm('¿Seguro desea eliminar el usuario completamente?', callBackDelUsrFn, 250, 150, '', 'Eliminar Usuario');
        }

        function callBackDelUsrFn(confirmed) {
            if (confirmed) {
                $find('<%= GrdUsers.ClientID%>').get_masterTableView().fireCommand("DeleteUsr", RowIndex);
            }
        }

        function onSendMailPass(index) {
            RowIndex = index;
            radconfirm('¿Enviar el enlace de restablecimiento de contrase&ntilde;a ?', callBackSendMailPassFn, 250, 150, '', 'Enviar Restablecimiento Contrase&ntilde;a');
        }

        function callBackSendMailPassFn(confirmed) {
            if (confirmed) {
                $find('<%= GrdUsers.ClientID%>').get_masterTableView().fireCommand("SendPasswordMail", RowIndex);
            }
        }

        function onAuthorizedUser(index) {
            RowIndex = index;
            radconfirm('¿Autorizar Usuario?', callBackAuthorizedUserFn, 250, 150, '', 'Usuario Desautorizado');
        }

        function callBackAuthorizedUserFn(confirmed) {
            if (confirmed) {
                $find('<%= GrdUsers.ClientID%>').get_masterTableView().fireCommand("Authorized", RowIndex);
            }
        }

        function onNotAuthorizedUser(index) {
            RowIndex = index;
            radconfirm('¿Desautorizar Usuario?', callBackNotAuthorizedUserFn, 250, 150, '', 'Usuario Autorizado');
        }

        function callBackNotAuthorizedUserFn(confirmed) {
            if (confirmed) {
                $find('<%= GrdUsers.ClientID%>').get_masterTableView().fireCommand("Authorized", RowIndex);
            }
        }

        function onLockedOut(index) {
            RowIndex = index;
            radconfirm('¿Desbloquear Usuario?', callBackLockedOutFn, 350, 250, '', 'Usuario Bloqueado');
        }

        function callBackLockedOutFn(confirmed) {
            if (confirmed) {
                $find('<%= GrdUsers.ClientID%>').get_masterTableView().fireCommand("LockedOut", RowIndex);
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
                    $find('<%= GrdUsers.ClientID%>').get_masterTableView().fireCommand("SetPassword", argument);
                }
            } else {
                radalert("Error: No se ingres&oacute; ninguna contrase&ntilde;a, por lo tanto no se ejecutar&aacute; ninguna acci&oacute;n.", 350, 250, "Cambiar Contrase&ntilde;a");
            }
        }

    </script>
</telerik:RadCodeBlock>

<telerik:RadSkinManager ID="SkinMan" runat="server" Skin="Bootstrap" />
<telerik:RadAjaxLoadingPanel ID="LoadingPanel" runat="server" Skin="MetroTouch" />
<telerik:RadWindowManager ID="WinMan" runat="server" />

<telerik:RadAjaxManager ID="AjaxManager" runat="server">
    <ClientEvents OnRequestStart="onRequestStart" />
    <AjaxSettings>
        <telerik:AjaxSetting AjaxControlID="GrdPatients">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdPatients" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="RPL_PatientData" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnSave">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdPatients" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="RPL_PatientData" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnClear">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdPatients" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="RPL_PatientData" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        
        <telerik:AjaxSetting AjaxControlID="CmbPatientsLabs">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="RPL_Labs" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="RPL_LabsDetails" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdPatientLabs">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdPatientLabs" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="RPL_LabsDetails" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdLabDetails">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdLabDetails" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="RPL_LabsDetails" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>

        <telerik:AjaxSetting AjaxControlID="GrdDoctors">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdDoctors" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="PRV_DoctorInfo" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnDrSave">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdDoctors" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="PRV_DoctorInfo" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="BtnDrClear">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdDoctors" LoadingPanelID="LoadingPanel" />
                <telerik:AjaxUpdatedControl ControlID="PRV_DoctorInfo" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdUsers">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdUsers" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="CmbDnnUsers">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdUsers_Roles" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
        <telerik:AjaxSetting AjaxControlID="GrdUsers_Roles">
            <UpdatedControls>
                <telerik:AjaxUpdatedControl ControlID="GrdUsers_Roles" LoadingPanelID="LoadingPanel" />
            </UpdatedControls>
        </telerik:AjaxSetting>
    </AjaxSettings>
</telerik:RadAjaxManager>


<div class="dnnForm dnnEdit dnnClear" id="dnnEdit">

    <telerik:RadTabStrip ID="PrincipalTabs" runat="server" MultiPageID="RM_Pages" CssClass="BottomMargin">
        <Tabs>
            <telerik:RadTab Text="Pacientes" Value="0" Selected="true" />
            <telerik:RadTab Text="Laboratorios" Value="1" />
            <telerik:RadTab Text="Doctors" Value="2" />
            <telerik:RadTab Text="Users" Value="3" />
        </Tabs>
    </telerik:RadTabStrip>

    <telerik:RadMultiPage ID="RM_Pages" runat="server">
        <telerik:RadPageView ID="RPV_Patients" runat="server" Selected="true">
            <telerik:RadGrid ID="GrdPatients" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowFilteringByColumn="true" AllowPaging="true"
                OnNeedDataSource="GrdPatients_NeedDataSource" OnItemCommand="GrdPatients_ItemCommand" GroupingSettings-CaseSensitive="false">
                <PagerStyle AlwaysVisible="true" />
                <MasterTableView DataKeyNames="Id" CommandItemDisplay="Top" NoMasterRecordsText="No existen pacientes creados." PageSize="5">
                    <CommandItemSettings RefreshText="Refrescar" ShowAddNewRecordButton="false" />
                    <PagerStyle PageSizes="10" Mode="NumericPages" PagerTextFormat="{4} P&aacute;gina {0} de {1}, Registro del {2} al {3} de {5}"  PageSizeLabelText="Registros"
                        NextPageToolTip="P&aacute;gina Siguiente" PrevPageToolTip="P&aacute;gina Anterior" FirstPageToolTip="Primera P&aacute;gina" 
                        LastPageToolTip="&Uacute;ltima P&aacute;gina" />
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="Actions" AllowFiltering="false" HeaderStyle-Width="25" >
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewData" CommandName="ViewData" Text="" CssClass="RadAButton rbLink rbIcon rbView" runat="server" ToolTip="Ver Datos" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Id" Display="false" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="PersonalId" HeaderText="Cédula" HeaderStyle-Width="175" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="LastName" HeaderText="Apellido" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="FirstName" HeaderText="Nombre" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="Email" HeaderText="Correo"  HeaderStyle-Width="250" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="Gender_Desc" HeaderText="Género"  HeaderStyle-Width="150" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="Years" HeaderText="Edad" HeaderStyle-Width="50" AllowFiltering="false"  />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br /><br />
            <telerik:RadPageLayout ID="RPL_PatientData" runat="server" CssClass="PageLayout">
                <Rows>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="LblPersonalId" runat="server" Text="Cédula" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtPersonalId" runat="server" MaxLength="21" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="LblGender" runat="server" Text="Género" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadComboBox ID="CmbGender" runat="server" EmptyMessage="Seleccione un Género">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="M" Text="Masculino" />
                                        <telerik:RadComboBoxItem Value="F" Text="Femenino" />
                                        <telerik:RadComboBoxItem Value="O" Text="Otro" />
                                    </Items>
                                </telerik:RadComboBox>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="LblBirthday" runat="server" Text="Fecha Nacimiento" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadDatePicker ID="DP_Birthday" runat="server" DateInput-DateFormat="dd/MMM/yyyy" Culture="es-ES" Calendar-ShowRowHeaders="false" MinDate="01/01/1900" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblFistName" runat="server" Text="Primer Nombre" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtFirstName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblSecondName" runat="server" Text="Segundo Nombre" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtSecondName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblLastName" runat="server" Text="Apellido Paterno" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtLastName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblSecondLastName" runat="server" Text="Apellido Materno" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtSecondLastName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblPhone" runat="server" Text="Teléfono" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtPhone" runat="server" MaxLength="10" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblCellPhone" runat="server" Text="Celular" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtCellPhone" runat="server" MaxLength="10" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblWhatsapp" runat="server" Text="Whatsapp" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtWhatsapp" runat="server" MaxLength="10" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="LblEmail" runat="server" Text="Correo Electrónico" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtEmail" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="12">
                                <telerik:RadLabel ID="LblAddress" runat="server" Text="Dirección" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtAddress" runat="server" TextMode="MultiLine" Width="90%" Height="100px" MaxLength="250" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="6">
                                <telerik:RadButton ID="BtnSave" runat="server" Text="Guardar" CssClass="BtnSave BtnIcon" OnClick="BtnSave_Click" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="6">
                                <telerik:RadButton ID="BtnClear" runat="server" Text="Limpiar" CssClass="BtnClear BtnIcon" OnClick="BtnClear_Click" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RPV_Labs" runat="server">
            <telerik:RadPageLayout ID="RPL_Labs" runat="server" CssClass="PageLayout">
                <Rows>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="12" SpanLg="10" SpanMd="8" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="LblPatients" runat="server" Text="Paciente:" Font-Bold="true" />
                                <telerik:RadComboBox ID="CmbPatientsLabs" runat="server" Width="400px" DataValueField="Id" EmptyMessage="Seleccione un Paciente"
                                    OnSelectedIndexChanged="CmbPatientsLabs_SelectedIndexChanged" Filter="Contains" DataTextField="DisplayName" RenderMode="Lightweight"
                                    CssClass="cmbDetails" Font-Size="Small" AutoPostBack="true">
                                    <HeaderTemplate>
                                        <table style="width: 100%; text-align: left; font-size: small">
                                            <tr>
                                                <td width="30%">Cédula</td>
                                                <td width="70%">Nombre</td>
                                            </tr>
                                        </table>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <table style="width: 100%; text-align: left; font-size: small">
                                            <tr>
                                                <td width="30%"><%# DataBinder.Eval(Container.DataItem, "PersonalId") %></td>
                                                <td width="70%"><%# DataBinder.Eval(Container.DataItem, "DisplayName") %> </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </telerik:RadComboBox>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel9" runat="server" Font-Bold="true" Text="Cédula:" />
                                <telerik:RadLabel ID="LblPatPersonalId" runat="server" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Nombre:" />
                                <telerik:RadLabel ID="LblPatName" runat="server" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel5" runat="server" Font-Bold="true" Text="Genero:" />
                                <telerik:RadLabel ID="LblPatGender" runat="server" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel7" runat="server" Font-Bold="true" Text="Edad:" />
                                <telerik:RadLabel ID="LblPatYears" runat="server" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel6" runat="server" Font-Bold="true" Text="Teléfono:" />
                                <telerik:RadLabel ID="LblPatPhone" runat="server" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel10" runat="server" Font-Bold="true" Text="Celular:" />
                                <telerik:RadLabel ID="LblPatCellPhone" runat="server" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel12" runat="server" Font-Bold="true" Text="Whatsapp:" />
                                <telerik:RadLabel ID="LblPatWhatsapp" runat="server" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3" SpanLg="2" SpanMd="4" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel14" runat="server" Font-Bold="true" Text="Correo:" />
                                <telerik:RadLabel ID="LblPatEmail" runat="server" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
            <telerik:RadPageLayout ID="RPL_LabsDetails" runat="server" CssClass="PageLayout">
                <Rows>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="12" SpanLg="10" SpanMd="8" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Laboratorios:" Font-Bold="true" />
                                <div style="max-width: 100%; overflow: auto;">
                                    <telerik:RadGrid ID="GrdPatientLabs" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowFilteringByColumn="true" 
                                        Culture="es-ES" Width="99%" OnNeedDataSource="GrdPatientLabs_NeedDataSource" OnItemDataBound="GrdPatientLabs_ItemDataBound" 
                                        OnItemCommand="GrdPatientLabs_ItemCommand" OnPreRender="GrdPatientLabs_PreRender" OnInsertCommand="GrdPatientLabs_InsertCommand" 
                                        OnUpdateCommand="GrdPatientLabs_UpdateCommand" OnDeleteCommand="GrdPatientLabs_DeleteCommand" AllowPaging="true" GroupingSettings-CaseSensitive="false">
                                        <MasterTableView NoMasterRecordsText="No hay laboratorios creados." CommandItemDisplay="Top" PageSize="5" DataKeyNames="Id">
                                            <EditFormSettings EditColumn-UpdateText="Actualizar" EditColumn-CancelText="Cancelar" />
                                            <CommandItemSettings RefreshText="Refrescar" AddNewRecordText="Añadir Laboratorio" />
                                            <PagerStyle PageSizes="10" Mode="NumericPages" PagerTextFormat="{4} P&aacute;gina {0} de {1}, Registro del {2} al {3} de {5}"  PageSizeLabelText="Registros"
                                                NextPageToolTip="P&aacute;gina Siguiente" PrevPageToolTip="P&aacute;gina Anterior" FirstPageToolTip="Primera P&aacute;gina" 
                                                LastPageToolTip="&Uacute;ltima P&aacute;gina" AlwaysVisible="true"/>
                                            <Columns>
                                                <telerik:GridEditCommandColumn HeaderStyle-Width="25px" UniqueName="EditColumn" EditText="Editar" />
                                                <telerik:GridTemplateColumn UniqueName="Actions" AllowFiltering="false" HeaderStyle-Width="25" >
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="ViewData" CommandName="ViewData" Text="" CssClass="RadAButton rbLink rbIcon rbView" runat="server" ToolTip="Ver Detalles" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="Id" ReadOnly="true" HeaderText="N°" AllowFiltering="false"/>
                                                <telerik:GridBoundColumn DataField="PatientId" Display="false" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="DoctorId" Display="false" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="Observation" Display="false" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="Doctor" HeaderText="Doctor" ReadOnly="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%" />
                                                <telerik:GridDropDownColumn DataField="DoctorId" UniqueName="LstDoctors" HeaderText="Doctor" Visible="false" FilterListOptions="AllowAllFilters">
                                                    <ColumnValidationSettings EnableRequiredFieldValidation="true" RequiredFieldValidator-ErrorMessage="Este campo es requerido" RequiredFieldValidator-CssClass="requiredError" />
                                                </telerik:GridDropDownColumn>
                                                <telerik:GridBoundColumn DataField="LabName" HeaderText="Laboratorio" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%" >
                                                    <ColumnValidationSettings EnableRequiredFieldValidation="true" RequiredFieldValidator-ErrorMessage="Este campo es requerido" RequiredFieldValidator-CssClass="requiredError" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="Status" HeaderText="Estado" ReadOnly="true" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%" />
                                                <telerik:GridDateTimeColumn DataField="CreateDate" UniqueName="CreateDate" HeaderText="Fecha" ReadOnly="true" DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" AllowFiltering="false" />
                                                <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn"  HeaderStyle-Width="50px" />
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="12" SpanLg="10" SpanMd="8" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel2" runat="server" Text="Detalles de Laboratorio:" Font-Bold="true" />
                                <div style="max-width: 100%; overflow: auto;">
                                    <telerik:RadGrid ID="GrdLabDetails" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowFilteringByColumn="true" 
                                        Culture="es-ES" Width="99%" OnNeedDataSource="GrdLabDetails_NeedDataSource" OnPreRender="GrdLabDetails_PreRender" 
                                        OnItemDataBound="GrdLabDetails_ItemDataBound" OnInsertCommand="GrdLabDetails_InsertCommand" OnUpdateCommand="GrdLabDetails_UpdateCommand" 
                                        OnDeleteCommand="GrdLabDetails_DeleteCommand" AllowPaging="true"  GroupingSettings-CaseSensitive="false">
                                        <MasterTableView NoMasterRecordsText="No hay detalles creados." CommandItemDisplay="Top" PageSize="5" DataKeyNames="Id">
                                            <CommandItemSettings RefreshText="Refrescar" AddNewRecordText="Añadir Resultado" />
                                            <PagerStyle PageSizes="10" Mode="NumericPages" PagerTextFormat="{4} P&aacute;gina {0} de {1}, Registro del {2} al {3} de {5}"  PageSizeLabelText="Registros"
                                                NextPageToolTip="P&aacute;gina Siguiente" PrevPageToolTip="P&aacute;gina Anterior" FirstPageToolTip="Primera P&aacute;gina" 
                                                LastPageToolTip="&Uacute;ltima P&aacute;gina" AlwaysVisible="true"/>
                                            <Columns>
                                                <telerik:GridEditCommandColumn HeaderStyle-Width="50px" UniqueName="EditColumn"/>
                                                <telerik:GridBoundColumn DataField="Id" Display="false" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="LaboratoryId" Display="false" ReadOnly="true" />
                                                <telerik:GridBoundColumn DataField="LabDetail" HeaderText="Descripción" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%">
                                                    <ColumnValidationSettings EnableRequiredFieldValidation="true" RequiredFieldValidator-ErrorMessage="Este campo es requerido" RequiredFieldValidator-CssClass="requiredError" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="LabResult" HeaderText="Resultado" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%" />
                                                <telerik:GridBoundColumn DataField="LabReference" HeaderText="Valor Referencia" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%" />
                                                <telerik:GridBoundColumn DataField="LabUnit" HeaderText="Unidad" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                                                    ShowFilterIcon="false" FilterControlWidth="90%" />
                                                <telerik:GridCheckBoxColumn DataField="Remark" HeaderText="Remarcar" HeaderStyle-Width="75" AllowFiltering="false" />
                                                <telerik:GridButtonColumn CommandName="Delete" Text="Borrar" UniqueName="DeleteColumn" HeaderStyle-Width="50px" />
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </div>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="12" SpanLg="10" SpanMd="8" SpanSm="6" SpanXs="6">
                                <telerik:RadLabel ID="RadLabel3" runat="server" Text="Observaciones del Laboratorio:" Font-Bold="true" />
                                <telerik:RadTextBox ID="TxtObservation" runat="server" TextMode="MultiLine" MaxLength="250" Width="100%" Height="100px" Enabled="false"/>
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadButton ID="BtnSaveObservation" runat="server" Text="Guardar Observación" CssClass="BtnIcon BtnSave" OnClick="BtnSaveObservation_Click" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadButton ID="BtnProcess" runat="server" Text="Procesar Laboratorio" CssClass="BtnIcon BtnProcess" OnClientClicking="OnClientClicking" OnClick="BtnProcess_Click" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadButton ID="BtnExport" runat="server" Text="Generar PDF" CssClass="BtnIcon BtnExport" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadButton ID="BtnClearResult" runat="server" Text="Limpiar Resultados" CssClass="BtnIcon BtnClear" OnClick="BtnClearResult_Click" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
        </telerik:RadPageView>
        <telerik:RadPageView ID="RPV_Doctors" runat="server">
            <telerik:RadGrid ID="GrdDoctors" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowFilteringByColumn="true" AllowPaging="true"
                OnNeedDataSource="GrdDoctors_NeedDataSource" OnItemCommand="GrdDoctors_ItemCommand" GroupingSettings-CaseSensitive="false">
                <PagerStyle AlwaysVisible="true" />
                <MasterTableView DataKeyNames="Id" CommandItemDisplay="Top" NoMasterRecordsText="No existen doctores creados." PageSize="5">
                    <CommandItemSettings RefreshText="Refrescar" ShowAddNewRecordButton="false" />
                    <PagerStyle PageSizes="10" Mode="NumericPages" PagerTextFormat="{4} P&aacute;gina {0} de {1}, Registro del {2} al {3} de {5}"  PageSizeLabelText="Registros"
                        NextPageToolTip="P&aacute;gina Siguiente" PrevPageToolTip="P&aacute;gina Anterior" FirstPageToolTip="Primera P&aacute;gina" 
                        LastPageToolTip="&Uacute;ltima P&aacute;gina" />
                    <Columns>
                        <telerik:GridTemplateColumn UniqueName="Actions" AllowFiltering="false" HeaderStyle-Width="25" >
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewData" CommandName="ViewData" Text="" CssClass="RadAButton rbLink rbIcon rbView" runat="server" ToolTip="Ver Datos" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridBoundColumn DataField="Id" Display="false" ReadOnly="true" />
                        <telerik:GridBoundColumn DataField="PersonalId" HeaderText="Cédula" HeaderStyle-Width="100" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="DrCode" HeaderText="Código" HeaderStyle-Width="50" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%"  />
                        <telerik:GridBoundColumn DataField="DrRegistry" HeaderText="Registro" HeaderStyle-Width="50" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%"  />
                        <telerik:GridBoundColumn DataField="LastName" HeaderText="Apellido" HeaderStyle-Width="150" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="FirstName" HeaderText="Nombre" HeaderStyle-Width="150" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="Email" HeaderText="Correo"  HeaderStyle-Width="150" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                        <telerik:GridBoundColumn DataField="Specialty" HeaderText="Especialidad"  HeaderStyle-Width="150" CurrentFilterFunction="Contains" AutoPostBackOnFilter="true" 
                            ShowFilterIcon="false" FilterControlWidth="90%" />
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br /><br />
            <telerik:RadPageLayout ID="PRV_DoctorInfo" runat="server" CssClass="PageLayout">
                <Rows>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="RadLabel8" runat="server" Text="Cédula" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrPersonalId" runat="server" MaxLength="21" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="RadLabel11" runat="server" Text="Género" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadComboBox ID="CmbDrGender" runat="server" EmptyMessage="Seleccione un Género">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="M" Text="Masculino" />
                                        <telerik:RadComboBoxItem Value="F" Text="Femenino" />
                                        <telerik:RadComboBoxItem Value="O" Text="Otro" />
                                    </Items>
                                </telerik:RadComboBox>
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="RadLabel13" runat="server" Text="Fecha Nacimiento" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadDatePicker ID="DP_DrBirthday" runat="server" DateInput-DateFormat="dd/MMM/yyyy" Culture="es-ES" Calendar-ShowRowHeaders="false" MinDate="01/01/1900" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel15" runat="server" Text="Primer Nombre" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrFirstName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel16" runat="server" Text="Segundo Nombre" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtDrSecondName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel17" runat="server" Text="Apellido Paterno" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrLastName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel18" runat="server" Text="Apellido Materno" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtDrSecondLastName" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="RadLabel24" runat="server" Text="Código de Doctor" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrCode" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="RadLabel25" runat="server" Text="Registro" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrRegistry" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="4">
                                <telerik:RadLabel ID="RadLabel26" runat="server" Text="Especialidad" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrSpecialty" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel19" runat="server" Text="Teléfono" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtDrPhone" runat="server" MaxLength="10" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel20" runat="server" Text="Celular" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtDrCellPhone" runat="server" MaxLength="10" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel21" runat="server" Text="Whatsapp" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtDrWhatsapp" runat="server" MaxLength="10" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="3">
                                <telerik:RadLabel ID="RadLabel22" runat="server" Text="Correo Electrónico" Font-Bold="true" CssClass="Required"/>
                                <telerik:RadTextBox ID="TxtDrEmail" runat="server" MaxLength="50" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="12">
                                <telerik:RadLabel ID="RadLabel23" runat="server" Text="Dirección" Font-Bold="true"/>
                                <telerik:RadTextBox ID="TxtDrAddress" runat="server" TextMode="MultiLine" Width="90%" Height="100px" MaxLength="250" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                    <telerik:LayoutRow>
                        <Columns>
                            <telerik:LayoutColumn Span="6">
                                <telerik:RadButton ID="BtnDrSave" runat="server" Text="Guardar" CssClass="BtnSave BtnIcon" OnClick="BtnDrSave_Click" />
                            </telerik:LayoutColumn>
                            <telerik:LayoutColumn Span="6">
                                <telerik:RadButton ID="BtnDrClear" runat="server" Text="Limpiar" CssClass="BtnClear BtnIcon" OnClick="BtnDrClear_Click" />
                            </telerik:LayoutColumn>
                        </Columns>
                    </telerik:LayoutRow>
                </Rows>
            </telerik:RadPageLayout>
        </telerik:RadPageView>
        
        <telerik:RadPageView ID="RPV_DnnUsers" runat="server">
            <telerik:RadTabStrip ID="tabs" runat="server" MultiPageID="pages" SelectedIndex="0" OnClientTabSelected="onTabSelected">
                <Tabs>
                    <telerik:RadTab Text="Usuarios" PageViewID="users" />
                    <telerik:RadTab Text="Roles de Usuarios" PageViewID="userRolesPage" />
                </Tabs>
            </telerik:RadTabStrip>

            <telerik:RadMultiPage ID="pages" runat="server" SelectedIndex="0">
                <telerik:RadPageView ID="users" runat="server">
                    <div class="gridLayout">
                        <telerik:RadGrid ID="GrdUsers" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowPaging="True" PageSize="10"
                            OnItemDataBound="GrdUsers_ItemDataBound" OnItemUpdated="GrdUsers_ItemUpdated" OnUpdateCommand="GrdUsers_UpdateCommand"
                            OnItemCommand="GrdUsers_ItemCommand" OnNeedDataSource="GrdUsers_NeedDataSource" OnInsertCommand="GrdUsers_InsertCommand" AllowFilteringByColumn="true"
                            CssClass="NormalGrid" >

                            <HeaderStyle BackColor="#0075bc" ForeColor="#FFFFFF" />
                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <GroupingSettings CaseSensitive="false" />

                            <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top" DataKeyNames="USERID,USERNAME" ClientDataKeyNames="USERNAME" TableLayout="Fixed"
                                NoDetailRecordsText="No hay registros secundarios para mostrar." NoMasterRecordsText="No hay registros para mostrar.">
                                <CommandItemSettings RefreshText="Refrescar" AddNewRecordText="Agregar Usuario" />
                                <RowIndicatorColumn Visible="False" />
                                <ExpandCollapseColumn Created="True" />
                                <Columns>
                                    <telerik:GridEditCommandColumn UniqueName="EditCommandColumn" EditText="Editar" HeaderStyle-Width="50" ItemStyle-CssClass="CustomCommands" />
                                    <telerik:GridTemplateColumn HeaderText="" UniqueName="Actions" AllowFiltering="false" HeaderStyle-Width="100" ItemStyle-CssClass="CustomCommands">
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
                                    <telerik:GridBoundColumn DataField="AUTHORIZED" HeaderText="Autorizado" UniqueName="AUTHORIZED" ReadOnly="true" HeaderStyle-Width="90"
                                        ShowFilterIcon="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" FilterControlWidth="70px" />
                                    <telerik:GridBoundColumn DataField="LOCKED_OUT" HeaderText="" UniqueName="LOCKED_OUT" ReadOnly="true" Display="false" />
                                    <telerik:GridBoundColumn DataField="IS_ONLINE" HeaderText="Online" UniqueName="IS_ONLINE" ReadOnly="true" Display="false" />
                                    <telerik:GridBoundColumn DataField="PASSWORD" HeaderText="Contraseña" UniqueName="PASSWORD" Display="false">
                                        <ColumnValidationSettings EnableRequiredFieldValidation="true">
                                            <RequiredFieldValidator Font-Size="Smaller" ForeColor="Red" ErrorMessage="Este campo es requerido." />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridButtonColumn CommandName="Delete" UniqueName="DeleteColumn"  HeaderStyle-Width="50px" />
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
                                                        <telerik:GridBoundColumn DataField="ROL_NAME" UniqueName="ROL_NAME" HeaderText="Rol" HeaderStyle-Width="420px" ReadOnly="true" />
                                                        <telerik:GridDateTimeColumn DataField="EFFECTIVE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                                                            HeaderText="Fecha Creación" DataFormatString="{0:MM/dd/yyyy}" ReadOnly="true" HeaderStyle-Width="210px" HtmlEncode="false" />
                                                        <telerik:GridDateTimeColumn DataField="EXPIRE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                                                            HeaderText="Fecha Expiración" DataFormatString="{0:MM/dd/yyyy}" HeaderStyle-Width="210px" EditDataFormatString="MM/dd/yyyy" />
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
                            OnDeleteCommand="GrdUsers_Roles_DeleteCommand" PageSize="50" CssClass="GrdSchedules" Width="600px" Culture="es-ES" >

                            <MasterTableView DataKeyNames="ROL_ID" NoMasterRecordsText="No hay registros para mostrar." AutoGenerateColumns="False" CommandItemDisplay="Top"
                                NoDetailRecordsText="No hay registros secundarios para mostrar.">
                                <CommandItemSettings AddNewRecordText="Agregar Rol" RefreshText="Refrescar" />
                            </MasterTableView>

                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top"
                                NoDetailRecordsText="No hay registros secundarios para mostrar." NoMasterRecordsText="No hay registros para mostrar.">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="ROL_ID" UniqueName="ROL_ID" ReadOnly="true" Display="false" />
                                    <telerik:GridBoundColumn DataField="ROL_NAME" UniqueName="ROL_NAME" HeaderText="Rol" HeaderStyle-Width="420px" ReadOnly="true" />
                                    <telerik:GridDropDownColumn DataField="ROL_NAME" UniqueName="lstRoles" HeaderText="Rol" Visible="false" FilterListOptions="AllowAllFilters" />
                                    <telerik:GridDateTimeColumn DataField="EFFECTIVE_DATE" DataType="System.DateTime" PickerType="DatePicker"
                                        HeaderText="Fecha Creación" DataFormatString="{0:dd/MMM/yyyy}" ReadOnly="true" HeaderStyle-Width="190px" HtmlEncode="false" />
                                    <telerik:GridDateTimeColumn DataField="EXPIRE_DATE" DataType="System.DateTime" PickerType="DatePicker" 
                                        HeaderText="Fecha Expiración" DataFormatString="{0:dd/MMM/yyyy}" HeaderStyle-Width="190px" EditDataFormatString="dd/MMM/yyyy" />
                                    <telerik:GridButtonColumn CommandName="Delete" Text="Delete" HeaderStyle-Width="50px" />
                                </Columns>
                            </MasterTableView>
                            <StatusBarSettings LoadingText="Cargando..." ReadyText="Listo" />
                        </telerik:RadGrid>
                    </div>
                </telerik:RadPageView>
            </telerik:RadMultiPage>

        </telerik:RadPageView>
    </telerik:RadMultiPage>
</div>


