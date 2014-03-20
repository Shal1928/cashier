package it.q02.asocp.database;

import it.q02.asocp.utils.SystemHelper;
import liquibase.Liquibase;
import liquibase.database.DatabaseConnection;
import liquibase.database.jvm.JdbcConnection;
import liquibase.exception.LiquibaseException;
import liquibase.resource.ClassLoaderResourceAccessor;
import liquibase.resource.FileSystemResourceAccessor;
import liquibase.resource.ResourceAccessor;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.session.SqlSessionFactoryBuilder;

import java.io.File;
import java.io.IOException;
import java.net.URL;
import java.util.Properties;

/**
 * User: aleksander at  16.03.14, 11:33
 * Сингелтон, при старте приложения проходит инициализацию
 * Предназначен для получения инстансов DataBaseService.
 */
public class DataBaseProvider {

    private static DataBaseProvider instance;
    private String configurationURL;
    private String propertiesURL;
    private SqlSessionFactory factory;
    private String updateScript;
    private ResourceAccessor accessor;

    public static void setInstance(DataBaseProvider provider){
        instance = provider;
    }

    public static DataBaseProvider getInstance() {
        return instance;
    }

    public void setConfigurationURL(String configurationURL) {
        if(configurationURL!=null && SystemHelper.isWindows()){
            configurationURL = configurationURL.replaceAll("://",":///").replace(File.separatorChar, '/');
        }
        this.configurationURL = configurationURL;
    }


    public void setPropertiesURL(String propertiesURL) {
        if(propertiesURL!=null && SystemHelper.isWindows()){
            propertiesURL = propertiesURL.replaceAll("://",":///").replace(File.separatorChar, '/');
        }
        this.propertiesURL = propertiesURL;
    }

    public void setUpdateScriptPath(String scriptPath) {
        this.updateScript = scriptPath;
    }

    public void setResourceAccessor(ResourceAccessor accessor) {

        this.accessor = accessor;
    }


    public void createConnection() {
        SqlSessionFactoryBuilder builder = new SqlSessionFactoryBuilder();
        Properties properties = new Properties();
        try {
            if(propertiesURL!=null && !propertiesURL.isEmpty()){
                properties.load(new URL(propertiesURL).openStream());
            }
            this.factory = builder.build(new URL(configurationURL).openStream(), properties);
        } catch (IOException e) {
            throw new RuntimeException("ошибка создания подключения к СУБД", e);
        }

    }

    public void updateDatabase() {
        try {
            SqlSession session = factory.openSession();
            DatabaseConnection connection = new JdbcConnection(session.getConnection());
            Liquibase updater = new Liquibase(updateScript, this.accessor, connection);
            updater.update(null);
        } catch (LiquibaseException e) {
            throw new RuntimeException("Ошибка обновления СУБД", e);
        }
    }

    private SqlSession createSession(){
        return factory.openSession();
    };

    public static DataBaseService createService(){
       return new DataBaseServiceImpl(DataBaseProvider.getInstance().createSession());
    };

    private static class DataBaseServiceImpl implements DataBaseService {

        private final SqlSession session;

        public DataBaseServiceImpl(SqlSession session) {
            this.session = session;
        }


        @Override
        public void commitTransaction() {
            this.session.commit();
        }

        @Override
        public void revertTransaction() {
            this.session.rollback();
        }


        @Override
        public SqlSession openSession() {
            return this.session;
        }

        @Override
        public void closeSession() {
            this.session.close();
        }

        @Override
        public <T> T getMapper(Class<T> clazz) {
            return session.getMapper(clazz);
        }
    }

    @Override
    public String toString() {
        return "DataBaseProvider{" +
                "updateScript='" + updateScript + '\'' +
                ", propertiesURL='" + propertiesURL + '\'' +
                ", configurationURL='" + configurationURL + '\'' +
                "} " + super.toString();
    }
}
