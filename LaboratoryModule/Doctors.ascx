<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Doctors.ascx.cs" Inherits="Jar.Dnn.LaboratoryModule.Doctors" %>
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
    </ajaxsettings>
</telerik:RadAjaxManager>

<div class="dnnForm dnnEdit dnnClear" id="dnnEdit">
    <telerik:RadGrid ID="GrdDoctors" runat="server" RenderMode="Lightweight" AutoGenerateColumns="false" AllowFilteringByColumn="true" AllowPaging="true"
        OnNeedDataSource="GrdDoctors_NeedDataSource" OnItemCommand="GrdDoctors_ItemCommand" GroupingSettings-CaseSensitive="false">
        <pagerstyle alwaysvisible="true" />
        <mastertableview datakeynames="Id" commanditemdisplay="Top" nomasterrecordstext="No existen doctores creados." pagesize="5">
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
        </mastertableview>
    </telerik:RadGrid>
    <br />
    <br />
    <telerik:RadPageLayout ID="PRV_DoctorInfo" runat="server" CssClass="PageLayout">
        <rows>
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
        </rows>
    </telerik:RadPageLayout>
</div>
