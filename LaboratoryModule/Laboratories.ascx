<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Laboratories.ascx.cs" Inherits="Jar.Dnn.LaboratoryModule.Laboratories" %>
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

    </script>
</telerik:RadCodeBlock>

<telerik:RadSkinManager ID="SkinMan" runat="server" Skin="Bootstrap" />
<telerik:RadAjaxLoadingPanel ID="LoadingPanel" runat="server" Skin="MetroTouch" />
<telerik:RadWindowManager ID="WinMan" runat="server" />

<telerik:RadAjaxManager ID="AjaxManager" runat="server">
    <ClientEvents OnRequestStart="onRequestStart" />
    <AjaxSettings>
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
    </AjaxSettings>
</telerik:RadAjaxManager>


<div class="dnnForm dnnEdit dnnClear" id="dnnEdit">
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
</div>