<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="visitors_list.aspx.cs" Inherits="LobbyManager.pages.visitors_list" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Histórico de Visitas</h1>
            </div>
        </div>

        <div class="dataTable_wrapper">
            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                <thead>
                    <tr>
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
                            <tr class="gradeU">
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

        <div class="modal fade" id="imagesDlg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel">Importar desde escáner</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Imágenes disponibles
                            </div>
                            <div class="panel-body">
                                <div class="dataTable_wrapper">
                                    Por favor, haga el escaneo del documento antes de contniuar.
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button class="btn btn-primary" runat="server" Text="Importar imágenes" />
                    </div>
                </div>
            </div>
        </div>
        <asp:SqlDataSource ID="SqlDataSourceVisitors" runat="server"
            ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"
            SelectCommand="SELECT vis_date, vis_department, vis_name, vis_lastname, vis_internal_contact, dep_name FROM [tbl_vis_visitors], tbl_dep_departments where dep_id = vis_department"></asp:SqlDataSource>
    </form>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
</asp:Content>
