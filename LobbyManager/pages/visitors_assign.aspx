<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="visitors_assign.aspx.cs" Inherits="LobbyManager.pages.visitors_assign" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <style type="text/css">
        tfoot input {
            width: 100%;
            padding: 3px;
            box-sizing: border-box;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Visitantes en Espera</h1>
            </div>
        </div>
        <div class="dataTable_wrapper table-responsive" runat="server" id="tableContainer">
            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Fecha</th>
                        <th>Departamento</th>
                        <th>Nombre</th>
                        <th>Apellido</th>
                        <th>Contacto</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceVisitors">
                        <ItemTemplate>
                            <tr class="gradeU" onclick="showDetails();">
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_id") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_date") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "dep_name") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_name") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_lastname") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_internal_contact") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <div class="modal fade" id="detailsDlg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="lblName"></h4>
                    </div>
                    <div class="modal-body">
                        <div class="dataTable_wrapper table-responsive" runat="server" id="Div1">
                            <table class="table table-striped table-bordered table-hover">
                                <tr>
                                    <th>
                                        <label for="lblComp">Compañía</label>
                                    </th>
                                    <td>
                                        <input style="width:100%;" type="text" id="lblComp" disabled="disabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label for="lblDate">Fecha de Ingreso</label>
                                    </th>
                                    <td>
                                        <input style="width:100%;" type="text" id="lblDate" disabled="disabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label for="lblDep">Departamento a Visitar</label>
                                    </th>
                                    <td>
                                        <input style="width:100%;" type="text" id="lblDep" disabled="disabled" />
                                    </td>
                                </tr>
                                <tr>
                                    <th>
                                        <label for="lblContact">Contacto Interno</label>
                                    </th>
                                    <td>
                                        <input style="width:100%;" type="text" id="lblContact" disabled="disabled" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server"
            ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"
            SelectCommand="SELECT vis_id, vis_date, vis_department, vis_name, vis_lastname, vis_internal_contact, dep_name 
                            FROM [tbl_vis_visitors], tbl_dep_departments 
                            where dep_id = vis_department order by vis_id desc">
        </asp:SqlDataSource>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
    <script src="../bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="../bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>
    <script>
        $(document).ready(function () {
           $('#dataTables-example').DataTable({
                responsive: true,
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "TODOS"]]
            });
        });

        function showDetails() {
            $('#lblName').text('Mauricio Montoya');
            $('#lblComp').val('M-Robotics Latin America');
            $('#lblDate').val('15/12/2015 04:35:53 p.m.');
            $('#lblDep').val('Informática');
            $('#lblContact').val('Alexis Guardado');
            $('#detailsDlg').modal('show');
        }

        function hideDetails() {
            $('#detailsDlg').modal('hide');
        }
    </script>
    
</asp:Content>
