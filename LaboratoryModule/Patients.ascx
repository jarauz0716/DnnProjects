<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Patients.ascx.cs" Inherits="Jar.Dnn.LaboratoryModule.Patients" %>
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

    </script>
</telerik:RadCodeBlock>

<telerik:RadSkinManager ID="SkinMan" runat="server" Skin="Bootstrap" />
<telerik:RadAjaxLoadingPanel ID="LoadingPanel" runat="server" Skin="MetroTouch" />
<telerik:RadWindowManager ID="WinMan" runat="server" />

<telerik:RadAjaxManager ID="AjaxManager" runat="server">
    <clientevents onrequeststart="onRequestStart" />
    <ajaxsettings>
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
    </ajaxsettings>
</telerik:RadAjaxManager>

<div class="dnnForm dnnEdit dnnClear" id="dnnEdit">
    <telerik:RadGrid ID="GrdPatients" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowFilteringByColumn="true" AllowPaging="true"
        OnNeedDataSource="GrdPatients_NeedDataSource" OnItemCommand="GrdPatients_ItemCommand" GroupingSettings-CaseSensitive="false">
        <pagerstyle alwaysvisible="true" />
        <mastertableview datakeynames="Id" commanditemdisplay="Top" nomasterrecordstext="No existen pacientes creados." pagesize="5">
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
                </mastertableview>
    </telerik:RadGrid>
    <br />
    <br />
    <telerik:RadPageLayout ID="RPL_PatientData" runat="server" CssClass="PageLayout">
        <rows>
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
                </rows>
    </telerik:RadPageLayout>
</div>
