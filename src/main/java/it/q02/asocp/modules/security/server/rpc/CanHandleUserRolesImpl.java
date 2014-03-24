package it.q02.asocp.modules.security.server.rpc;

import com.google.gwt.user.server.rpc.RemoteServiceServlet;
import it.q02.asocp.modules.base.client.data.RoleMap;
import it.q02.asocp.modules.base.client.helpers.UserRoles;
import it.q02.asocp.modules.security.client.rpc.CanHandleUserRoles;
import it.q02.asocp.users.ExecutionContextStorage;
import it.q02.asocp.utils.RoleMapInfoProvider;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import java.util.LinkedList;
import java.util.List;

import static it.q02.asocp.modules.base.client.helpers.CommonHelper.isNullOrEmpty;

/**
 *
 */
public class CanHandleUserRolesImpl extends RemoteServiceServlet implements CanHandleUserRoles {
    @Override
    public List<RoleMap> getRoleMaps() {
        UserRoles roles = ExecutionContextStorage.getContext().getUserInfo().getUserRoles();
        if (isNullOrEmpty(roles) || roles.isOnlyAut()) return null;

        Context context;
        Context envContext = null;
        RoleMapInfoProvider roleMapInfoProvider = null;
        try {
            context = new InitialContext();
            envContext = (Context) context.lookup("java:comp/env");
            roleMapInfoProvider = (RoleMapInfoProvider) envContext.lookup("utils/RoleMapInfo");
        } catch (NamingException e) {
            e.printStackTrace();
        }

        List<RoleMap> roleMaps = new LinkedList();
        for (String role : roles.getIgnoreAut())
            roleMaps.add(roleMapInfoProvider.getMap(role));

        return roleMaps;
    }
}