package it.q02.asocp.modules.base.client.data;

import com.google.gwt.user.client.rpc.IsSerializable;

/**
 *
 */
public class RoleMap implements IsSerializable {

    private String role;
    private String title;
    private String url;


    public RoleMap() {
        //
    }

    public RoleMap(String roleName, String title, String url) {
        this.role = roleName;
        this.title = title;
        this.url = url;
    }


    public String getRole() {
        return role;
    }

    public void setRole(String role) {
        this.role = role;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }
}
