<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="stations_form.aspx.cs" Inherits="LobbyManager.pages.stations_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:hiddenfield ID="selectedID" runat="server" value="" />
        <asp:hiddenfield ID="recordOption" runat="server" value="" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Administración de Estaciones de Enrolamiento</h1>
            </div>
        </div>
        <div class="alert alert-warning" id="msgAccess" style="display:none;">
            Modo de acceso restringido.
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel">
                    <div class="panel-body">
                        <div class="row">
                            <div  style="float: right; margin-bottom: 10px;">
                                <button type="button" class="btn btn-primary" onclick="showForm();" >Agregar Estación</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <table class="table table-striped table-bordered table-hover" id="equipmentDataTable">
                                    <thead>
                                        <tr>
                                            <th>Windows Service ID</th>
                                            <th>Nombre de Máquina</th>
                                            <th>Estado</th>
                                            <th>&nbsp;</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                                    <ItemTemplate>
                                                        <tr class="gradeU">
                                                            <td><%# DataBinder.Eval(Container.DataItem, "dev_id") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "dev_name") %></td>
                                                            <td><%# (DataBinder.Eval(Container.DataItem, "dev_status").Equals("1"))?"Activo":"Inactivo" %></td>
                                                            <td>
                                                                <%# "<button type=\"button\" class=\"btn btn-info\" onclick=\"editRecord('" + 
                                                                                    DataBinder.Eval(Container.DataItem, "dev_id").ToString().Trim() + "','" + 
                                                                                    DataBinder.Eval(Container.DataItem, "dev_name").ToString().Trim() + "','" + 
                                                                                    DataBinder.Eval(Container.DataItem, "dev_status").ToString().Trim() +
                                                                    "');\"><i class=\"fa fa-pencil\"></i></button>" %>
                                                            </td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="deleteDlg" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">Confirmar acción</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            Eliminar Registro
                                        </div>
                                        <div class="panel-body">
                                            <div class="dataTable_wrapper">
                                                ¿Desea eliminar el registro? Puede agregarlo desde el formulario posteriormente.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button class="btn btn-primary" runat="server" Text="Aceptar" OnClientClick="deleteRecord()" />
                                    <asp:Button class="btn" runat="server" Text="Cancelar" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="addDlg" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">Estación de Enrolamiento</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            Datos Requeridos
                                        </div>
                                        <div class="panel-body">
                                            <fieldset id="fs_personalData" runat="server">
                                                <div class="alert alert-danger" id="msgFormError" style="display:none;"></div>
                                                <div class="form-group">
                                                    <label for="eqTypeSelect">Windows Service ID</label>
                                                    <input runat="server" id="txt_serviceID" class="form-control" placeholder="" />
                                                </div>
                                                <div class="form-group">
                                                    <label>Nombre de Máquina</label>
                                                    <input runat="server" id="txt_name" class="form-control" placeholder="" />
                                                </div>
                                                <div class="form-group">
                                                    <input runat="server" id="chk_active" type="checkbox" />Activo
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button class="btn btn-success" runat="server" OnClientClick="if (!validateForm()) return false;" Text="Guardar" OnClick="saveItem" />
                                    <asp:Button class="btn btn-danger" ID="btnDelete" runat="server" Text="Eliminar" OnClientClick="deleteRecord();" />
                                    <asp:Button class="btn btn-default" runat="server" data-dismiss="modal" Text="Cancelar" ID="btnCancelForm" OnClick="btnCancelForm_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <asp:SqlDataSource ID="SqlDataSourceList" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
    <!-- DataTables JavaScript -->
    <script src="../bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="../bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        var currentrecord = 0;

        $(document).ready(function() {
            /*$('#equipmentDataTable').DataTable({
                    responsive: true
            });*/

            if (<%= Request.QueryString["access"] %> != '0'){
                $('#msgAccess').show();
                $('.btn').attr("disabled", "disabled");
            }
        });

        function validateForm(){
            var res = true;

            $("#msgFormError").slideUp(function (){
                if ($('#<%=txt_name.ClientID%>').val() === ""){
                    $("#msgFormError").text('Nombre requerido');
                    $("#msgFormError").slideDown();
                    res = false;
                }

                if ($('#<%=txt_serviceID.ClientID%>').val() === ""){
                    $("#msgFormError").text('Se requiere el identificador en el Servicio de Windows Instalado');
                    $("#msgFormError").slideDown();
                    res = false;
                }
            });

            return res;
        }

        function showForm() {
            $('#<%=recordOption.ClientID%>').val('A');
            $('#<%=txt_name.ClientID%>').val('');
            $('#<%=txt_serviceID.ClientID%>').val('');
            $('#<%=btnDelete.ClientID%>').hide();
            $('#addDlg').modal('show');
            setFocus();
        }

        function editRecord(serv, name, status) {
            currentrecord = serv;
            $('#<%=recordOption.ClientID%>').val('E');
            $('#<%=selectedID.ClientID%>').val(serv);
            $('#<%=btnDelete.ClientID%>').show();
            $('#<%=txt_name.ClientID%>').val(name);
            $('#<%=txt_serviceID.ClientID%>').val(serv);
            $('#<%=chk_active.ClientID%>').prop('checked', (status === '1')? true : false);
            $('#addDlg').modal('show');
            setFocus();
        }

        function setFocus() {
            setTimeout(function(){ $( "#<%=txt_serviceID.ClientID%>" ).focus(); }, 500);
        }

        function deleteRecord() {
            PageMethods.deleteRecord(currentrecord, OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                //alert(response);
                hideMsg();
                window.location.reload();
            }
        }

        function showMsg() {
            currentrecord = $('#<%=selectedID.ClientID%>').val();
            deleteRecord();
        }

        function hideMsg() {
            $('#deleteDlg').modal('hide');
        }
    </script>
</asp:Content>
