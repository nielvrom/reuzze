using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ReUzze.Helpers;
using ReUzze.Models;

namespace ReUzze.Helpers
{
    public class reuzzeRepository
    {
        private reuzzeCS2 entities;

        #region Constructors

        public reuzzeRepository():this(ObjectContextPerHttpRequest.Context){
        }

        private reuzzeRepository(reuzzeCS2 context)
        {
            this.entities = context;
        }
 
        #endregion

        #region Query Methods

        // GET ALL ENTITIES
        public IQueryable<entity> GetAllEntities()
        {
            return from entity in entities.entities
                   orderby entity.entity_title
                   select entity;
        }

        public IQueryable<medium> GetAllMedia()
        {
            return from media in entities.media
                   orderby media.entity.entity_title
                   select media;
        }

        public IQueryable<bid> GetAllBids()
        {
            return from bid in entities.bids
                   orderby bid.entity.entity_created
                   select bid;
        }

        public IQueryable<entity> GetEntitiesFromUser(int userid)
        {
            return from entity in entities.entities
                   where entity.user_id.Equals(userid)
                   orderby entity.entity_title
                   select entity;
        }

        public IQueryable<entity> GetLastEntities()
        {
            return from entity in entities.entities.Take(10)
                   orderby entity.entity_id descending
                   select entity;
        }

        public IQueryable<user> GetLastUsers()
        {
            return from user in entities.users.Take(10)
                   orderby user.user_id descending
                   select user;
        }

        public IQueryable<bid> GetLastBids()
        {
            return from bid in entities.bids.Take(10)
                   orderby bid.bid_id descending
                   select bid;
        }

        public IQueryable<bid> GetBidsFromUser(int userid)
        {
            return from bid in entities.bids
                   where bid.user_id.Equals(userid)
                   select bid;
        }

        // GET ENTITY BY SEND ID
        public IEnumerable<entity> GetEntityByID(Int64 id)
        {
            return from entity in entities.entities
                   where entity.entity_id.Equals(id)
                   select entity;
        }

        public IQueryable<favorite> GetFavoritesFromUser(int userid)
        {
            return from favorite in entities.favorites
                   where favorite.user_id.Equals(userid)
                   select favorite;
        }


        public entity GetEntity(Int64 id)
        {
            return entities.entities.SingleOrDefault(entit => entit.entity_id == id);
        }


        public user GetUser(Int32 id)
        {
            return entities.users.SingleOrDefault(u => u.user_id == id);
        }

        public void AddMedia(long entityid, string path, string mimetype)
        {
            try
            {
                medium media = new medium();
                media.entity_id = entityid;
                media.medium_type = "i";
                media.medium_url = path;
                media.medium_mimetype = mimetype;
                entities.media.Add(media);
                Save();
            }
            catch (Exception ex)
            {

            }
        }

        public void AddFavorite(favorite favorite)
        {
            try
            {
                entities.favorites.Add(favorite);
                Save();
            }
            catch(Exception ex)
            {

            }
        }

        public favorite GetFavorite(Int32 userid, Int64 entityid)
        {
            return entities.favorites.SingleOrDefault(f => f.user_id == userid && f.entity_id == entityid);
        }

        public void DeleteFavorite(favorite favorite)
        {
            try
            {
                entities.favorites.Remove(favorite);
                Save();
            }
            catch (Exception ex)
            {

            }
        }

        public void DeleteEntity(entity entity)
        {
            try
            {
                // REMOVE THE BIDS FROM THE ENTITIES
                foreach(bid item in entity.bids)
                {
                    entities.bids.Remove(item);
                }

                // REMOVE THE ENTITY
                entities.entities.Remove(entity);

                Save();
            }
            catch (Exception ex)
            {

            }
        }

        public void Save()
        {
            entities.SaveChanges();
        }

        #endregion
    }
}