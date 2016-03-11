<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="role_menu.aspx.cs" Inherits="LobbyManager.pages.role_menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <table>
                    <tr>
                        <td>
                            <button type="button" class="btn btn-primary btn-lg"><i class="fa fa-arrow-circle-o-left fa-2x"></i></button>
                        </td>
                        <td>
                            <h1 class="page-header" style="margin-left: 10px;">Opciones de Menu - <asp:Label runat="server" id="lblRoleTitle" Text=""></asp:Label></h1>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:Label ID="lblTitle" runat="server">Usuarios Registrados en el Sistema</asp:Label>
                    </div>
                    <div class="panel-body">
                        <table class="table table-striped table-bordered table-hover" id="equipmentDataTable">
                            <thead>
                                <tr>
                                    <th class="info" rowspan="2" style="vertical-align: middle;">Opción de Menú</th>
                                    <th class="info" colspan="3" style="text-align: center;">Nivel de Permiso</th>
                                </tr>
                                <tr>
                                    <th style="text-align: center;">Lectura y Escritura</th>
                                    <th style="text-align: center;">Solo Lectura</th>
                                    <th style="text-align: center;">Ninguno</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                            <ItemTemplate>
                                                <tr class="gradeU">
                                                    <td><%# DataBinder.Eval(Container.DataItem, "menu_label").ToString().Trim() %></td>
                                                    <td style="text-align: center;"><input type="radio" name="rad_<%# DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim() %>" value="0" /></td>
                                                    <td style="text-align: center;"><input type="radio" name="rad_<%# DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim() %>" value="1" /></td>
                                                    <td style="text-align: center;"><input type="radio" name="rad_<%# DataBinder.Eval(Container.DataItem, "menu_id").ToString().Trim() %>" value="2" /></td>
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
    </form>
    <asp:SqlDataSource ID="SqlDataSourceList" runat="server" ConnectionString="<%$ ConnectionStrings:SykesVisitorsDB %>" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Footer" runat="server">
    <!-- DataTables JavaScript -->
    <script src="../bower_components/datatables/media/js/jquery.dataTables.min.js"></script>
    <script src="../bower_components/datatables-plugins/integration/bootstrap/3/dataTables.bootstrap.min.js"></script>

    <!-- Page-Level Demo Scripts - Tables - Use for reference -->
    <script>
        function deleteRecord() {
            PageMethods.deleteRecord(currentrecord, OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                //alert(response);
                hideMsg();
                window.location.reload();
            }
        }
    </script>
</asp:Content>
