<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="current_visits.aspx.cs" Inherits="LobbyManager.pages.current_visits" %>

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
                <h1 class="page-header">Visitas en Curso</h1>
            </div>
        </div>
            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Foto</th>
                        <th>Fecha</th>
                        <th>Departamento</th>
                        <th>Nombre</th>
                        <th>Apellido</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceVisitors">
                        <ItemTemplate>
                            <tr class="gradeU">
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_id") %></td>
                                <td>
                                    <img runat="server" id="img_front" src='<%# @"data:image/bmp;base64," + DataBinder.Eval(Container.DataItem, "img_profile") %>' height="48" />
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <td><%# DataBinder.Eval(Container.DataItem, "vis_date").ToString().Split(' ')[0] %></td>
                                        </tr>
                                        <tr>
                                            <td><%# DataBinder.Eval(Container.DataItem, "vis_date").ToString().Split(' ')[1] + " " + DataBinder.Eval(Container.DataItem, "vis_date").ToString().Split(' ')[2] %></td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    <table>
                                        <tr>
                                            <th>
                                                <span style="color: darkblue;"><%# DataBinder.Eval(Container.DataItem, "dep_name").ToString().ToUpper() %></span>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%# DataBinder.Eval(Container.DataItem, "vis_internal_contact").ToString().ToUpper() %>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_name") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "vis_lastname") %></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td colspan="2" style="font-size: x-small; font-weight: bold;">TAR:<%# DataBinder.Eval(Container.DataItem, "vis_visitor_card") %></td>
                                        </tr>
                                        <tr>
                                            <td><%# "<button type=\"button\" class=\"btn btn-warning\" onclick=\"showDetails(" + DataBinder.Eval(Container.DataItem, "vis_id") + ");\"><i class=\"fa fa-list\"></i></button>" %></td>
                                            <td><%# (!DataBinder.Eval(Container.DataItem, "vis_with_equipment").ToString().Equals("1")) ? "<button type=\"button\" class=\"btn btn-danger\" style=\"margin-left: 10px;\" onclick=\"finishMsg(" + DataBinder.Eval(Container.DataItem, "vis_id") + ");\"><i class=\"fa fa-times\"></i></button>" : "<button type=\"button\" class=\"btn btn-danger\" style=\"margin-left: 10px;\" onclick=\"revEq(" + DataBinder.Eval(Container.DataItem, "vis_id") + ");\"><i class=\"fa fa-times\"></i></button>" %></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>

        <div class="modal fade" id="finishDlg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel">Confirmar acción</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Finalizar Visita
                            </div>
                            <div class="panel-body">
                                <div class="dataTable_wrapper">
                                    Para finalizar la visita, haga clic en "Finalizar" de lo contrario haga clic en "Cancelar"
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button class="btn btn-danger" runat="server" Text="Finalizar Visita" OnClientClick="finishVisit();" />
                        <asp:Button class="btn btn-default" runat="server" Text="Cancelar"  data-dismiss="modal" />
                    </div>
                </div>
            </div>
        </div>

        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server"
            ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"
            SelectCommand="SELECT vis_id, vis_date, vis_department, vis_name, vis_lastname, vis_internal_contact, vis_with_equipment, vis_visitor_card, dep_name, img_profile 
                            FROM [tbl_vis_visitors], tbl_dep_departments, tbl_img_images 
                            where dep_id = vis_department and vis_status = 2 and img_visitor = vis_id order by vis_id desc">
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

        function showDetails(visitor) {
            window.location = "visit_termination.aspx?visitor=" + visitor;
        }

        var visitor = 0;

        function finishVisit() {
            PageMethods.finishVisit(visitor, OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                window.location.reload();
            }
        }

        function finishMsg(visID) {
            visitor = visID;
            $('#finishDlg').modal('show');
        }

        function revEq(visID) {
            window.location = "equipment_exit.aspx?visitor=" + visID;
        }
    </script>
    
</asp:Content>
