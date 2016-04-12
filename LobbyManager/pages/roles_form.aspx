<%@ Page Title="" Language="C#" MasterPageFile="~/masters/Main.Master" AutoEventWireup="true" CodeBehind="roles_form.aspx.cs" Inherits="LobbyManager.pages.roles_form" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainHolder" runat="server">
    <form role="form" runat="server">
        <asp:hiddenfield ID="selectedID" runat="server" value="" />
        <asp:hiddenfield ID="option" runat="server" value="" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div class="row">
            <div class="col-lg-12">
                <h1 class="page-header">Administración de Roles</h1>
            </div>
        </div>
        <div class="alert alert-warning" id="msgAccess" style="display:none;">
            Modo de acceso restringido.
        </div>
        <div class="row">
            <div class="col-lg-12">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <asp:Label ID="lblTitle" runat="server">Roles Registrados en el Sistema</asp:Label>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div  style="float: right; margin-bottom: 10px;">
                                <button type="button" class="btn btn-primary" onclick="showForm();" >Agregar Nuevo Rol</button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-lg-12">
                                <table class="table table-striped table-bordered table-hover" id="equipmentDataTable">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Nombre</th>
                                            <th>Nivel de Acceso</th>
                                            <th>Estado</th>
                                            <th>Opciones</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <asp:Repeater ID="Repeater2" runat="server" DataSourceID="SqlDataSourceList">
                                                    <ItemTemplate>
                                                        <tr class="gradeU">
                                                            <td><%# DataBinder.Eval(Container.DataItem, "role_id").ToString().Trim() %></td>
                                                            <td><%# DataBinder.Eval(Container.DataItem, "role_name").ToString().Trim() %></td>
                                                            <td><%# (DataBinder.Eval(Container.DataItem, "role_level").ToString().Trim().Equals("0"))? "Administrador" : "Usuario" %></td>
                                                            <td><%# (DataBinder.Eval(Container.DataItem, "role_status").ToString().Trim().Equals("1"))? "Activo" : "Inactivo" %></td>
                                                            <td>
                                                                <%# "<button type=\"button\" class=\"btn btn-success\" onclick=\"showMenuDetails(" + DataBinder.Eval(Container.DataItem, "role_id") + ");\"><i class=\"fa fa-list\"></i></button>" %>
                                                                <%# "<button type=\"button\" class=\"btn btn-info\" onclick=\"editRecord('" + DataBinder.Eval(Container.DataItem, "role_id").ToString().Trim() + "','" + DataBinder.Eval(Container.DataItem, "role_name").ToString().Trim() + "','" + DataBinder.Eval(Container.DataItem, "role_level").ToString().Trim() + "','" + DataBinder.Eval(Container.DataItem, "role_status").ToString().Trim() + "');\"><i class=\"fa fa-pencil\"></i></button>" %>
                                                            </td>
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
                    <div class="modal fade" id="deleteDlg" tabindex="-1" role="dialog" aria-labelledby="delModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">Confirmar acción</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            Eliminar Registro
                                        </div>
                                        <div class="panel-body">
                                            <div class="dataTable_wrapper">
                                                ¿Desea eliminar el registro? Puede agregarlo desde el formulario posteriormente.
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button class="btn btn-primary" runat="server" Text="Aceptar" OnClientClick="deleteRecord()" />
                                    <asp:Button class="btn" runat="server" Text="Cancelar" data-dismiss="modal" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal fade" id="addDlg" tabindex="-1" role="dialog" aria-labelledby="addModalLabel" aria-hidden="true">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                    <h4 class="modal-title">Rol</h4>
                                </div>
                                <div class="modal-body">
                                    <div class="panel panel-default">
                                        <div class="panel-heading">
                                            Datos Requeridos
                                        </div>
                                        <div class="panel-body">
                                            <fieldset id="fs_personalData" runat="server">
                                                <div class="alert alert-warning" id="msgWarn" runat="server">
                                                    El campo 'Nombre' es obligatorio
                                                </div>
                                                <div class="form-group">
                                                    <label>Nombre</label>
                                                    <input runat="server" id="txt_name" class="form-control" placeholder="" />
                                                </div>
                                                <div class="form-group">
                                                    <label for="roleLevel">Nivel de Acceso</label>
                                                    <select runat="server" id="roleLevel" class="form-control">
                                                        <option value="1">Usuario</option>
                                                        <option value="0">Administrador</option>
                                                    </select>
                                                </div>
                                                <div class="form-group">
                                                    <input runat="server" id="chk_active" type="checkbox" />   Activo
                                                </div>
                                            </fieldset>
                                        </div>
                                    </div>
                                </div>
                                <div class="modal-footer">
                                    <asp:Button class="btn btn-success" runat="server" Text="Guardar" OnClick="saveItem" />
                                    <asp:Button class="btn btn-danger" ID="btnDelete" runat="server" Text="Eliminar" OnClientClick="showMsg();" />
                                    <asp:Button class="btn btn-default" runat="server" data-dismiss="modal" Text="Cancelar" ID="btnCancelForm" OnClick="btnCancelForm_Click"/>
                                </div>
                            </div>
                        </div>
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
        var currentrecord = 0;
        
        $(document).ready(function() {
            /*$('#equipmentDataTable').DataTable({
                    responsive: true
            });*/

            if (<%= Request.QueryString["access"] %> != '0'){
                $('#msgAccess').show();
                $('.btn').attr("disabled", "disabled");
            }
        });
        
        function showMenuDetails(role) {
            window.location = "role_menu.aspx?role=" + role;
        }

        function deleteRecord() {
            PageMethods.deleteRecord(currentrecord, OnSuccess);
            function OnSuccess(response, userContext, methodName) {
                //alert(response);
                hideMsg();
                window.location.reload();
            }
        }

        function showForm() {
            $('#<%=option.ClientID%>').val('A');
            $('#<%=txt_name.ClientID%>').val('');
            $('#<%=roleLevel.ClientID%>').val('1');
            $('#<%=btnDelete.ClientID%>').hide();
            $('#<%=chk_active.ClientID%>').prop('checked', true);
            $('#addDlg').modal('show');
            setFocus();
        }

        function editRecord(rec, name, level, status) {
            currentrecord = rec;
            $('#<%=option.ClientID%>').val('E');
            $('#<%=selectedID.ClientID%>').val(rec);
            $('#<%=btnDelete.ClientID%>').show();
            $('#<%=txt_name.ClientID%>').val(name);
            $('#<%=roleLevel.ClientID%>').val(level);
            $('#<%=chk_active.ClientID%>').prop('checked', (status === '1')? true : false);
            $('#addDlg').modal('show');
            setFocus();
        }

        function setFocus(){
            setTimeout(function(){ $( "#<%=txt_name.ClientID%>" ).focus(); }, 500);
        }

        function showMsg() {
            currentrecord = $('#<%=selectedID.ClientID%>').val();
            deleteRecord();
        }

        function hideMsg() {
            $('#deleteDlg').modal('hide');
        }
    </script>
</asp:Content>
