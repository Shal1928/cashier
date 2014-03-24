package it.q02.asocp.modules.security.client.rpc;

import com.google.gwt.user.client.rpc.AsyncCallback;
import it.q02.asocp.modules.base.client.data.RoleMap;
import it.q02.asocp.modules.base.client.helpers.UserRoles;

import java.util.Collection;
import java.util.List;

/**
 *
 */
public interface CanHandleUserRolesAsync {
    public void getRoleMaps(AsyncCallback<List<RoleMap>> async);
}
