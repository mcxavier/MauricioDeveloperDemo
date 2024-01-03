using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using Core.SharedKernel;
using Newtonsoft.Json;

namespace Infra.QueryCommands.Commands.Topics
{ 
    public class PublishApplicationLocation : IRequest<Response>
    {
        public string CompanyName { get; set; }
        public PublishApplicationLocationAddress Address { get; set; }
        public PublishApplicationLocationDocumentNumber DocumentNumber { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Enabled { get; set; }
        public Nullable<int> HandlingDays { get; set; }
        public string LocationCode { get; set; }
        public string LocationId { get; set; }
        public string Name { get; set; }
        public PublishApplicationLocationWorkingDays WorkingDays { get; set; }
        public string Phone { get; set; }
        public string Type { get; set; }

        public Nullable<int> PickupDeadline { get; set; }
        public Nullable<bool> PickupEnabled { get; set; }
        public Nullable<bool> ShipmentEnabled { get; set; }

        public DateTime UpdatedAt { get; set; }
    }

    public class PublishApplicationLocationAddress
    {
        public Nullable<Int32> AddressId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressName { get; set; }
        public string City { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string Country { get; set; }
        public Nullable<bool> Default { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Neighbourhood { get; set; }
        public string Number { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
    }

    public class PublishApplicationLocationOpeningHours
    {
        public string From { get; set; }
        public string To { get; set; }
    }


    public class PublishApplicationLocationDocumentNumber
    {
        public string Cnpj { get; set; }
        public string Ie { get; set; }
    }



    public class PublishApplicationLocationWorkingDays
    {
        public IList<PublishApplicationLocationOpeningHours> FRI { get; set; }

        public IList<PublishApplicationLocationOpeningHours> MON { get; set; }

        public IList<PublishApplicationLocationOpeningHours> SAT { get; set; }

        public IList<PublishApplicationLocationOpeningHours> THU { get; set; }

        public IList<PublishApplicationLocationOpeningHours> TUE { get; set; }

        public IList<PublishApplicationLocationOpeningHours> WED { get; set; }

    }
}
