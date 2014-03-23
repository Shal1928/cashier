package it.q02.asocp.modules.security.server.rpc;

import com.google.gwt.user.server.rpc.RemoteServiceServlet;
import it.q02.asocp.modules.security.client.rpc.CanHandleUserRoles;
import it.q02.asocp.users.ExecutionContextStorage;
import it.q02.asocp.modules.base.client.helpers.UserRoles;

import static it.q02.asocp.modules.base.client.helpers.CommonHelper.isNullOrEmpty;

/**
 *
 */
public class CanHandleUserRolesImpl extends RemoteServiceServlet implements CanHandleUserRoles {
    @Override
    public UserRoles getRoles() {
        return ExecutionContextStorage.getContext().getUserInfo().getUserRoles();
    }
}