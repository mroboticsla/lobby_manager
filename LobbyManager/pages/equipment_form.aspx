<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="equipment_form.aspx.cs" Inherits="LobbyManager.pages.equipment_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
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
                            <div class="col-lg-4">
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
                                    <div class="form-group">
                                        <asp:Button class="btn btn-primary" runat="server" Text="Guardar" OnClick="saveItem" />
                                        <button type="reset" class="btn btn-default">Cancelar</button>
                                    </div>
                                </fieldset>
                            </div>
                            <div class="col-lg-8">
                                <div class="dataTable_wrapper table-responsive">
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
                                            <asp:ScriptManager ID="ScriptManager1" runat="server">
                                            </asp:ScriptManager>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                                        <ItemTemplate>
                                                            <tr class="gradeU">
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
    $(document).ready(function() {
        $('#equipmentDataTable').DataTable({
                responsive: true
        });
    });
    </script>
</asp:Content>
