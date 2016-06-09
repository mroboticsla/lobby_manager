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
                <h1 class="page-header" runat="server" id="mainTitle">Visitantes en Espera</h1>
            </div>
        </div>
        <div class="alert alert-warning" id="msgAccess" style="display:none;">
                Modo de acceso restringido.
        </div>
        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Estaci&oacute;n</th>
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
                        <tr class="gradeU <%# DataBinder.Eval(Container.DataItem, "alert") %>">
                            <td><%# DataBinder.Eval(Container.DataItem, "vis_id") %></td>
                            <td><%# DataBinder.Eval(Container.DataItem, "log_user") %></td>
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
                            <td><%# "<button type=\"button\" class=\"btn btn-warning\" onclick=\"showDetails(" + DataBinder.Eval(Container.DataItem, "vis_id") + ");\"><i class=\"fa fa-list\"></i></button>" %></td>
                        </tr>

                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>

        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>">
        </asp:SqlDataSource>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
    <script src="../bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="../bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>
    <script>
        var isReadOnly = false;

        $(document).ready(function () {
           $('#dataTables-example').DataTable({
                responsive: true,
                "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "TODOS"]]
           });

           if (<%= (Request.QueryString["access"] != null)?Request.QueryString["access"] : "0" %> != '0'){
               isReadOnly = true;
               $('#msgAccess').show();
           }
        });

        function showDetails(visitor) {
            if (isReadOnly){
                window.location = "visit_consult.aspx?visitor=" + visitor;
            }else{
                window.location = "visitors_approve.aspx?visitor=" + visitor;
            }
            
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