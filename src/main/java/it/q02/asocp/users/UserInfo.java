package it.q02.asocp.users;

import java.util.List;

/**
 * User: aleksander at  16.03.14, 12:23
 * Описывает пользователя
 *
 */
public class UserInfo {

    private String userLogin;
    private String userName;

    private List<String> userRoles;

    public String getUserLogin() {
        return userLogin;
    }

    public void setUserLogin(String userLogin) {
        this.userLogin = userLogin;
    }

    public String getUserName() {
        return userName;
    }

    public void setUserName(String userName) {
        this.userName = userName;
    }

    public List<String> getUserRoles() {
        return userRoles;
    }

    public void setUserRoles(List<String> userRoles) {
        this.userRoles = userRoles;
    }
}