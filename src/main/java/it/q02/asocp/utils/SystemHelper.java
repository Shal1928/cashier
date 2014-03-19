package it.q02.asocp.utils;

/**
 * Проверка ОС
 */
public class SystemHelper {

    public static String getOS(){
        return System.getProperty("os.name");
    }

    public static boolean isWindows() {
        return (getOS().toLowerCase().indexOf("win") >= 0);
    }

    public static boolean isMac() {
        return (getOS().toLowerCase().indexOf("mac") >= 0);
    }

    public static boolean isUnix() {
        return (getOS().toLowerCase().indexOf("nux") >= 0);
    }
}
