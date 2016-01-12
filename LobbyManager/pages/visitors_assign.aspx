﻿<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="visitors_assign.aspx.cs" Inherits="LobbyManager.pages.visitors_assign" %>

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
                            <tr class="gradeU" onclick="showDetails('<%# DataBinder.Eval(Container.DataItem, "vis_id") %>');">
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

        function showDetails(visitor) {
            window.location = "visitors_approve.aspx?visitor=" + visitor;
        }
    </script>
    
</asp:Content>
