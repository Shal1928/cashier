package it.q02.asocp.modules.security.client.rpc;

import com.google.gwt.user.client.rpc.RemoteService;
import com.google.gwt.user.client.rpc.RemoteServiceRelativePath;
import it.q02.asocp.modules.base.client.data.RoleMap;

import java.util.List;

/**
 *
 */
@RemoteServiceRelativePath("CanHandleUserRoles")
public interface CanHandleUserRoles extends RemoteService {
    public List<RoleMap> getRoleMaps();
}
