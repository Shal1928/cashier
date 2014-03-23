package it.q02.asocp.modules.security.client.rpc;

import com.google.gwt.user.client.rpc.AsyncCallback;
import it.q02.asocp.modules.base.client.helpers.UserRoles;

/**
 *
 */
public interface CanHandleUserRolesAsync {
    public void getRoles(AsyncCallback<UserRoles> async);
}
