package it.q02.asocp.utils.impl;

import it.q02.asocp.users.UserInfo;
import it.q02.asocp.utils.UserInfoProvider;

import java.io.IOException;
import java.net.URL;
import java.util.Arrays;
import java.util.Properties;

/**
 * User: aleksander at  16.03.14, 14:49
 */
public class UserInfoProviderImpl implements UserInfoProvider {


    private Properties properties;
    private String file;

    public void setSourceFile(String file) {
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
            userInfo.setUserRoles(Arrays.asList(properties.getProperty(userLogin + ".roles").split(";")));
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
