declare @ChitId int

set @ChitId = 1

declare @Numberofmonths int, @Counter int

select @Numberofmonths = NumberofMonths from tblChits where ChitId = @ChitId

set @Counter = 1
while (@Counter <= @Numberofmonths)
begin
	insert into tblPayments (AuctionId, [Month])
	select Auctionid, @Counter from tblAuctions a
	set @Counter = @Counter + 1
end
