package it.q02.asocp.utils.impl;

import it.q02.asocp.users.UserInfo;
import it.q02.asocp.modules.base.client.helpers.UserRoles;
import it.q02.asocp.utils.SystemHelper;
import it.q02.asocp.utils.UserInfoProvider;

import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.Properties;

/**
 * User: aleksander at  16.03.14, 14:49
 */
public class UserInfoProviderImpl implements UserInfoProvider {


    private Properties properties;
    private String file;

    public void setSourceFile(String file) {
        if(SystemHelper.isWindows()){
            file = file.replaceAll("://",":///").replace(File.separatorChar, '/');
        }


        this.file = file;
        this.properties = new Properties();
        try {
            this.properties.load(new URL(file).openStream());
        } catch (IOException e) {
            throw new RuntimeException("error load user infos...",e);
        }
    }

    @Override
    public UserInfo getUserInfo(String userLogin) {

        if (properties.containsKey(userLogin + ".display")) {
            UserInfo userInfo = new UserInfo();
            userInfo.setUserLogin(userLogin);
            userInfo.setUserName(properties.getProperty(userLogin + ".display"));
            userInfo.setUserRoles(new UserRoles(properties.getProperty(userLogin + ".roles").split(";")));
            return userInfo;
        }else{
            return null;
        }
    }

    @Override
    public String toString() {
        return "UserInfoProviderImpl{" +
                "properties=" + properties +
                "file=" + file +
                "} " + super.toString();
    }
}
