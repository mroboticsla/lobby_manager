<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="equipment_form.aspx.cs" Inherits="LobbyManager.pages.equipment_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Registro de Equipo</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:Label ID="lblTitle" runat="server"></asp:Label>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div  style="float: right; margin-bottom: 10px;">
                                <button type="button" class="btn btn-primary" onclick="showForm();" >Agregar Equipo</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <table class="table table-striped table-bordered table-hover" id="equipmentDataTable">
                                    <thead>
                                        <tr>
                                            <th>Cantidad</th>
                                            <th>Tipo</th>
                                            <th>No. Serie</th>
                                            <th>Descripción</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                                    <ItemTemplate>
                                                        <tr class="gradeU" onclick="showMsg('<%# DataBinder.Eval(Container.DataItem, "reg_id") %>');">
                                                            <td><%# DataBinder.Eval(Container.DataItem, "reg_quantity") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "type_name") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "reg_serial") %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "reg_desc") %></td>
                                                        </tr>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </tbody>
                                </table>
                                *Toque sobre la lista para eliminar un registro.
                            </div>
                        </div>
                        <div class="row">
                            <div  style="float: right; margin-top: 10px;">
                                <asp:Button class="btn" runat="server" Text="Imprimir Viñetas" OnClick="btnExecutePrint_Click" ID="btnExecutePrint" />
                                <button type="button" class="btn btn-success" onclick="finishProc();">Finalizar</button>
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
                                    <h4 class="modal-title">Agregar Equipo</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            Formulario Ingreso de Equipo
                                        </div>
                                        <div class="panel-body">
                                            <fieldset id="fs_personalData" runat="server">
                                                <div class="alert alert-warning" id="msgWarn" runat="server">
                                                    El campo 'Descripción' es obligatorio
                                                </div>
                                                <div class="form-group">
                                                    <label for="eqTypeSelect">Tipo de Equipo</label>
                                                    <asp:DropDownList runat="server" ID="eqTypeSelect" class="form-control"
                                                        DataSourceID="SqlDataSourceEquipment" DataValueField="type_id" DataTextField="type_name">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="form-group">
                                                    <label>No. de Serie</label>
                                                    <input runat="server" id="txt_serial" class="form-control" placeholder="" />
                                                </div>
                                                <div class="form-group">
                                                    <label>Descripción</label>
                                                    <textarea runat="server" id="txt_desc" class="form-control" rows="3"></textarea>
                                                </div>
                                                <div class="form-group">
                                                    <label>Cantidad</label>
                                                    <input runat="server" id="txt_quantity" class="form-control" placeholder="1" />
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
            $('#equipmentDataTable').DataTable({
                    responsive: true
            });
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
            window.location="visitors.aspx?finish=true";
        }
    </script>
</asp:Content>
