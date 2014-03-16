package it.q02.asocp.utils;

import it.q02.asocp.users.UserInfo;

/**
 * User: aleksander at  16.03.14, 14:47
 */
public interface UserInfoProvider {

    public static class Provider{

        private static UserInfoProvider provider;

        public static void initProvider(UserInfoProvider provider){
           Provider.provider = provider;
        }

        public static UserInfo getUserInfo(String login){
            return provider.getUserInfo(login);
        }

    }

    public UserInfo getUserInfo(String userLogin);
}
