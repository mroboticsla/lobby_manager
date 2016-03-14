<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="departments_form.aspx.cs" Inherits="LobbyManager.pages.departments_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Administración de Departamentos</h1>
            </div>
        </div>
        <div class="alert alert-warning" id="msgAccess" style="display:none;">
            Modo de acceso restringido.
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:Label ID="lblTitle" runat="server">Departamentos Registrados en el Sistema</asp:Label>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div  style="float: right; margin-bottom: 10px;">
                                <button type="button" class="btn btn-primary" onclick="showForm();" >Agregar Departamento</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <table class="table table-striped table-bordered table-hover" id="equipmentDataTable">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>NOMBRE</th>
                                            <th>CONTACTO</th>
                                            <th>TELEFONO</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                                    <ItemTemplate>
                                                        <tr class="gradeU" onclick="showMsg('<%# DataBinder.Eval(Container.DataItem, "dep_id") %>');">
                                                            <td><%# DataBinder.Eval(Container.DataItem, "dep_id") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "dep_name") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "dep_contact") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "dep_phone") %></td>
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
                                    <h4 class="modal-title">Tipo de Documento</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            Datos Requeridos
                                        </div>
                                        <div class="panel-body">
                                            <fieldset id="fs_personalData" runat="server">
                                                <div class="alert alert-warning" id="msgWarn" runat="server">
                                                    El campo 'Nombre' es obligatorio
                                                </div>
                                                <div class="form-group">
                                                    <label for="eqTypeSelect">Nombre</label>
                                                    <input runat="server" id="txt_name" class="form-control" placeholder="" />
                                                </div>
                                                <div class="form-group">
                                                    <label>Contacto</label>
                                                    <input runat="server" id="txt_contact" class="form-control" placeholder="" />
                                                </div>
                                                <div class="form-group">
                                                    <label>Teléfono</label>
                                                    <input runat="server" id="txt_phone" class="form-control" placeholder="" />
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button class="btn btn-success" runat="server" Text="Guardar" OnClick="saveItem" />
                                    <asp:Button class="btn btn-danger" runat="server" data-dismiss="modal" Text="Cancelar" ID="btnCancelForm" OnClick="btnCancelForm_Click"/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>


    <asp:SqlDataSource ID="SqlDataSourceEquipment" runat="server"
        ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"
        SelectCommand="SELECT type_id, type_name FROM [tbl_type_equipment] WHERE type_status = 1"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSourceList" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"></asp:SqlDataSource>
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

        function deleteRecord() {
            PageMethods.deleteRecord(currentrecord, OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                //alert(response);
                hideMsg();
                window.location.reload();
            }
        }

        function showForm() {
            $('#addDlg').modal('show');
        }

        function showMsg(rec) {
            currentrecord = rec;
            $('#deleteDlg').modal('show');
        }

        function hideMsg() {
            $('#deleteDlg').modal('hide');
        }

        function finishProc() {
            if ((<%= approved.ToString().ToLower() %>))
            {
                window.location="visitors_approve.aspx?visitor=<%= visitorID.ToString().ToLower() %>&finish=true";
            }else{
                window.location="visitors.aspx?finish=true";
            }
            
        }
    </script>
</asp:Content>
