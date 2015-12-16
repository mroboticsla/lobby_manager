<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.master" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="LobbyManager.pages.admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="Server">
    <form runat="server" name="mainForm" id="mainform">
        <div id="wrapper">
            <div class="row">
                <div class="col-lg-12">
                    <h1 class="page-header">Consola de Administración</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-primary">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-comments fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge">
                                        <asp:Label ID="lbl_newCommentsCount" runat="server" Text="0"></asp:Label>
                                    </div>
                                    <div>Nuevos Comentarios</div>
                                </div>
                            </div>
                        </div>
                        <a data-toggle="modal" href="#commentsDlg">
                            <div class="panel-footer">
                                <span class="pull-left">Ver Detalles</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-green">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-tasks fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge">127</div>
                                    <div>Vistantes Registrados</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">Ver Detalles</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
                <div class="col-lg-4 col-md-6">
                    <div class="panel panel-red">
                        <div class="panel-heading">
                            <div class="row">
                                <div class="col-xs-3">
                                    <i class="fa fa-support fa-5x"></i>
                                </div>
                                <div class="col-xs-9 text-right">
                                    <div class="huge">0</div>
                                    <div>Errores Reportados</div>
                                </div>
                            </div>
                        </div>
                        <a href="#">
                            <div class="panel-footer">
                                <span class="pull-left">Ver Detalles</span>
                                <span class="pull-right"><i class="fa fa-arrow-circle-right"></i></span>
                                <div class="clearfix"></div>
                            </div>
                        </a>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-lg-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <i class="fa fa-bar-chart-o fa-fw"></i>Registro Global de Visitantes
                        </div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="panel-body">
                                    <div id="morris-donut-chart"></div>
                                    <a href="#" class="btn btn-default btn-block">Ver Detalles</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <i class="fa fa-bell fa-fw"></i>Visor de Notificaciones
                       
                        </div>
                        <div class="panel-body">
                            <div class="list-group">
                                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceNotifications">
                                    <ItemTemplate>
                                        <a href="#" class="list-group-item">
                                            <i class='<%# DataBinder.Eval(Container.DataItem, "type_class") %>'></i><%# DataBinder.Eval(Container.DataItem, "log_user") %> - <%# DataBinder.Eval(Container.DataItem, "type_label") %>

                                            <span class="pull-right text-muted small"><em><%# DataBinder.Eval(Container.DataItem, "log_date") %></em>
                                            </span>
                                        </a>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <a href="#" class="btn btn-default btn-block">Ver todas las notificaciones</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade" id="commentsDlg" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                        <h4 class="modal-title" id="myModalLabel">Nuevos Comentarios</h4>
                    </div>
                    <div class="modal-body">
                        <div class="panel panel-default">
                            <div class="panel-heading">
                                Nuevos Comentarios
                            </div>
                            <div class="panel-body">
                                <div class="dataTable_wrapper">
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                        <thead>
                                            <tr>
                                                <th>Fecha</th>
                                                <th>Usuario</th>
                                                <th>Descripción</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:Repeater ID="Repeater1" runat="server" DataSourceID="SqlDataSourceComments">
                                                <ItemTemplate>
                                                    <tr class="odd gradeX">
                                                        <td><%# DataBinder.Eval(Container.DataItem, "com_date") %></td>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "com_user") %></td>
                                                        <td><%# DataBinder.Eval(Container.DataItem, "com_description") %></td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button class="btn btn-primary" runat="server" OnClick="hideNewComments" Text="Cerrar" />
                    </div>
                </div>
            </div>
        </div>
    </form>

    <asp:SqlDataSource ID="SqlDataSourceComments" runat="server"
        ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"
        SelectCommand="SELECT TOP 5 CONVERT(VARCHAR(8), com_date, 3) AS com_date,com_user,com_description FROM [tbl_com_comments]"></asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSourceNotifications" runat="server"
        ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>"
        SelectCommand="SELECT TOP 10 CONVERT(VARCHAR, log_date, 0) AS log_date,UPPER(log_user) log_user,type_label, type_class 
                    FROM tbl_log_events, tbl_type_notifications
                    where log_notification = type_id ORDER BY log_id DESC"></asp:SqlDataSource>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="Server">
    <script src="../bower_components/raphael/raphael-min.js"></script>
    <script src="../bower_components/morrisjs/morris.min.js"></script>
    <script src="../js/morris-visitors.js"></script>
</asp:Content>
