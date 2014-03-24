package it.q02.asocp.utils.impl;

import it.q02.asocp.modules.base.client.data.RoleMap;
import it.q02.asocp.utils.RoleMapInfoProvider;

import java.util.Collection;
import java.util.HashSet;

import static it.q02.asocp.modules.base.client.helpers.CommonHelper.isNullOrEmpty;

/**
 *
 */
public class RoleMapInfoProviderImpl implements RoleMapInfoProvider {

    private Collection<RoleMap> roleMaps = new HashSet();

    @Override
    public RoleMap getMap(String role) {
        if(isNullOrEmpty(roleMaps)) return null;

        for(RoleMap roleMap : roleMaps)
            if(roleMap.getRole() != null && roleMap.getRole().equals(role))
                return roleMap;

        return null;
    }

    public void addMap(String role, String title, String url){
        roleMaps.add(new RoleMap(role, title, url));
    }
}
