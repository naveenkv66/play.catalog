namespace Play.Catalog.Service
{
    using Play.Catalog.Service.Dtos;
    using Play.Catalog.Service.Entities;
    public static class Extensions
    {
        public static ItemDto AsDTO(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.price, item.CreatedDate);
        }
        public static IEnumerable<ItemDto> AsListDTO(this IReadOnlyCollection<Item> itemList)
        {
            List<ItemDto> returnList = new List<ItemDto>();
            itemList.ToList().ForEach(x => returnList.Add(x.AsDTO()));
            return returnList;
        }
        public static Item AsEntity(this ItemDto itemDto)
        {
            return new Item()
            {
                Id = itemDto.Id,
                Name = itemDto.Name,
                Description = itemDto.Description,
                price = itemDto.price,
                CreatedDate = DateTimeOffset.UtcNow
            };
        }
    }
}