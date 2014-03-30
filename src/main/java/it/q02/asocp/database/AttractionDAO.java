package it.q02.asocp.database;

import it.q02.asocp.modules.base.client.data.Attraction;

import java.util.List;

/**
 * User: aleksander at  29.03.14, 18:25
 */
public interface AttractionDAO {

    public void create(Attraction attraction);
    public void update(Attraction attraction);

    public List<Attraction> find();

}
