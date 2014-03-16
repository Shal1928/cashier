package it.q02.asocp.database;


import org.apache.ibatis.session.SqlSession;

/**
 * User: aleksander at  16.03.14, 11:31
 */
public interface DataBaseService {

    public void commitTransaction();
    public void revertTransaction();
    public SqlSession getSession();

    public <T> T getMapper(Class<T> clazz);





}
