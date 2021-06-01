using AutoMapper;
using MongoDB.Driver;
using DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Interfaces;
using DAL.Configuration;
using DAL.Entities;

namespace DAL
{
    public class NewsRepository : INewsRepository
    {
        private readonly DBContext _context = null;
        private readonly IMapper _mapper;

        public NewsRepository(MongoDbConfig config, IMapper mapper)
        {
            _context = new DBContext(config);
            _mapper = mapper;
        }


        public async Task<IEnumerable<NewsDTO>> GetAllNewsAsync()
        {
            var news = await _context.News.Find(_ => true).ToListAsync();
            return _mapper.Map<IEnumerable<NewsDTO>>(news);
        }


        public async Task<NewsDTO> AddNews(NewsDTO item)
        {
            try
            {
                item.ID = Guid.NewGuid().ToString();
                await _context.News.InsertOneAsync(_mapper.Map<NewsEntity>(item));
                return await GetNewsById(item.ID);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<NewsDTO> GetNewsById(string externalId)
        {
            try
            {
                var result = await GetNewsEntityById(externalId);
                return _mapper.Map<NewsDTO>(result);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        private async Task<NewsEntity> GetNewsEntityById(string externalId)
        {
            try
            {
                var result = await _context.News
                                .Find(n => n.ID == externalId)
                                .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public async Task<bool> UpdateNews(string externalId, NewsDTO item)
        {
            try
            {
                var originalEntity = await GetNewsEntityById(externalId);
                var newsEntity = _mapper.Map<NewsEntity>(item);
                newsEntity.InternalId = originalEntity.InternalId;


                ReplaceOneResult actionResult = await _context.News.
                             ReplaceOneAsync(n => n.InternalId.Equals(newsEntity.InternalId),
                             newsEntity,
                             new ReplaceOptions { IsUpsert = false });

                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}
