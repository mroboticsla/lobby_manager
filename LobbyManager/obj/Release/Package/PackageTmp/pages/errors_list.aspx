<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="errors_list.aspx.cs" Inherits="LobbyManager.pages.errors_list" %>

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
                <h1 class="page-header">Histórico de Errores</h1>
            </div>
        </div>
        <div class="alert alert-warning" id="msgAccess" style="display:none;">
            Modo de acceso restringido.
        </div>
        <div class="dataTable_wrapper table-responsive" runat="server" id="tableContainer">
            <div style="float:right; margin-bottom: 15px">
                <button type="button" class="btn btn-primary" onclick="doTable();">Exportar Tabla a Excel</button>
                <asp:Button class="btn btn-primary" runat="server" Text="" OnClick="btnExportTable" style="display:none" ID="btnExecuteExport" />
            </div>
            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                <thead>
                    <tr>
                        <th>ID</th>
                        <th>Fecha</th>
                        <th>Usuario</th>
                        <th>Descripción</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceVisitors">
                        <ItemTemplate>
                            <tr class="gradeU">
                                <td><%# DataBinder.Eval(Container.DataItem, "com_id") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "com_date") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "com_user") %></td>
                                <td><%# DataBinder.Eval(Container.DataItem, "com_description") %></td>
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

        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>">
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

           if (<%= Request.QueryString["access"] %> != '0'){
               $('#msgAccess').show();
               $('.btn').attr("disabled", "disabled");
           }
        });

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
    
</asp:Content>
