<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="visitors_list.aspx.cs" Inherits="LobbyManager.pages.visitors_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    <link href="../Content/bootstrap-datetimepicker.css" rel="stylesheet" />
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
                <h1 class="page-header">Histórico de Visitas</h1>
            </div>
        </div>
        <div class="alert alert-warning" id="msgWarn" runat="server">
            Fechas no válidas
        </div>
        <div class="row">
            <div class="col-lg-4 col-md-4 col-sm-12">
                <div class="form-group">
                    <label>Desde:</label>
                    <div class='input-group date' id='dateFrom'>
                        <input type='text' class="form-control" runat="server" id="txt_initialDate" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-md-4 col-sm-12">
                <div class="form-group">
                    <label>Hasta:</label>
                    <div class='input-group date' id='dateTo'>
                        <input type='text' class="form-control" runat="server" id="txt_finalDate" />
                        <span class="input-group-addon">
                            <span class="glyphicon glyphicon-calendar"></span>
                        </span>
                    </div>
                </div>
            </div>
            <div class="col-lg-4 col-lg-4 col-md-4 col-sm-12">
                <div class="form-group">
                    <label>&nbsp;</label><br />
                    <asp:Button class="btn btn-info" runat="server" Text="Buscar en Rango de fechas" OnClick="DoSearch" CausesValidation="False" ID="btnSearch" />
                </div>
            </div>
        </div>
        <div class="dataTable_wrapper table-responsive" style="overflow-x: scroll;" runat="server" id="tableContainer">
            <div style="float: left; margin-bottom: 15px; margin-top: 15px;">
                <button type="button" class="btn btn-primary" onclick="doTable();">Exportar a Excel</button>
                <asp:Button class="btn btn-primary" runat="server" Text="" OnClick="btnExportTable" Style="display: none" ID="btnExecuteExport" />
            </div>
            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Fecha</th>
                        <th>Departamento</th>
                        <th>Nombre</th>
                        <th>Apellido</th>
                        <th>Contacto</th>
                        <th>Salida</th>
                        <th style="white-space: nowrap;">No. de Tarjeta Asignado</th>
                        <th style="white-space: nowrap;">Empleado Asociado</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceVisitors">
                        <ItemTemplate>
                            <tr class="gradeU" onclick="goTo('<%# DataBinder.Eval(Container.DataItem, "vis_id") %>');">
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_id") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_date") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "dep_name") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_name") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_lastname") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_internal_contact") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_checkout") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_visitor_card") %></td>
                                <td style="white-space: nowrap;"><%# DataBinder.Eval(Container.DataItem, "vis_in_charge") %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

        <div class="modal fade" id="completeDlg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title">Preparando la información</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Espere...
                            </div>
                            <div class="panel-body">
                                <div class="dataTable_wrapper">
                                    Se está procesando su solicitud.
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"></asp:SqlDataSource>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
    <script src="../bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="../bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>
    <script src="../Scripts/moment-with-locales.js"></script>
    <script src="../Scripts/bootstrap-datetimepicker.js"></script>
    <script>
        $(document).ready(function () {
            $('#dataTables-example').DataTable({
                responsive: true,
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "TODOS"]]
            });
        });

        function goTo(visit) {
            window.location = "visit_consult.aspx?visitor=" + visit;
        }

        function doTable() {
            showMsg();
            PageMethods.setHTML($('#<%= tableContainer.ClientID %>').html(), OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                //alert(response);
                setTimeout(hideMsg, 2000);
            }
        }

        function showMsg() {
            $('#completeDlg').modal('show');
        }

        function hideMsg() {
            document.getElementById("<%= btnExecuteExport.ClientID %>").click();
            $('#completeDlg').modal('hide');
        }
    </script>
    <script type="text/javascript">
        $(function () {
            $('#dateFrom,#dateTo').datetimepicker({
                locale: 'es',
                format: 'DD/MM/YYYY'
            });
        });
    </script>
</asp:Content>
