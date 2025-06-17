
export default function (mock) {
    const orders = [
        {
            OrderId: 1,
            MemberId: 101,
            MemberName:"AA",
            MemberPhone:987654321,
            PlanId: 201,
            PlanType: 2,
            PlanName: "健身月卡",
            OrderNumber: 202406170001,
            State: 1,
            Amount: 1200,
            Method: 2,
            Remark: "首次購買",
            CreateTime: "2025-06-17T10:01:00.123",
            UpdateTime: "2025-06-17T10:01:00.123"
        },
        {
            OrderId: 2,
            MemberId: 102,
            MemberName:"BB",
            MemberPhone:987654322,
            PlanId: 202,
            PlanType: 2,
            PlanName: "瑜伽季卡",
            OrderNumber: 202406170002,
            State: 2,
            Amount: 3200,
            Method: 2,
            Remark: "升級方案",
            CreateTime: "2025-06-16T14:20:30.456",
            UpdateTime: "2025-06-16T14:20:30.456"
        },
        {
            OrderId: 3,
            MemberId: 103,
            MemberName:"CC",
            MemberPhone:987654323,
            PlanId: 203,
            PlanType: 1,
            PlanName: "私人教練體驗",
            OrderNumber: 202406170003,
            State: 1,
            Amount: 600,
            Method: 1,
            Remark: "",
            CreateTime: "2025-06-15T09:10:05.789",
            UpdateTime: "2025-06-15T09:10:05.789"
        },
        {
            OrderId: 4,
            MemberId: 104,
            MemberName:"DD",
            MemberPhone:987654324,
            PlanId: 204,
            PlanType: 3,
            PlanName: "團體課半年卡",
            OrderNumber: 202406170004,
            State: 3,
            Amount: 5400,
            Method: 2,
            Remark: "現場付款",
            CreateTime: "2025-06-14T12:00:00.000",
            UpdateTime: "2025-06-14T12:00:00.000"
        },
        {
            OrderId: 5,
            MemberId: 105,
            MemberName:"EE",
            MemberPhone:987654325,
            PlanId: 205,
            PlanType: 1,
            PlanName: "健身單次票",
            OrderNumber: 202406170005,
            State: 1,
            Amount: 300,
            Method: 1,
            Remark: "贈送1次體驗",
            CreateTime: "2025-06-13T18:45:10.321",
            UpdateTime: "2025-06-13T18:45:10.321"
        },
        {
            OrderId: 6,
            MemberId: 106,
            MemberName:"FF",
            MemberPhone:987654326,
            PlanId: 206,
            PlanType: 2,
            PlanName: "瑜伽月卡",
            OrderNumber: 202406170006,
            State: 2,
            Amount: 1500,
            Method: 2,
            Remark: "",
            CreateTime: "2025-06-12T11:15:20.999",
            UpdateTime: "2025-06-12T11:15:20.999"
        },
        {
            OrderId: 7,
            MemberId: 107,
            MemberName:"GG",
            MemberPhone:987654327,
            PlanId: 207,
            PlanType: 1,
            PlanName: "舞蹈月卡",
            OrderNumber: 202406170007,
            State: 1,
            Amount: 1000,
            Method: 1,
            Remark: "推薦朋友加入",
            CreateTime: "2025-06-11T16:05:55.321",
            UpdateTime: "2025-06-11T16:05:55.321"
        },
        {
            OrderId: 8,
            MemberId: 108,
            MemberName:"HH",
            MemberPhone:987654328,
            PlanId: 208,
            PlanType: 3,
            PlanName: "健身年卡",
            OrderNumber: 202406170008,
            State: 1,
            Amount: 9600,
            Method: 2,
            Remark: "信用卡付款",
            CreateTime: "2025-06-10T08:30:45.555",
            UpdateTime: "2025-06-10T08:30:45.555"
        },
        {
            OrderId: 9,
            MemberId: 109,
            MemberName:"II",
            MemberPhone:987654329,
            PlanId: 209,
            PlanType: 2,
            PlanName: "私人教練三個月",
            OrderNumber: 202406170009,
            State: 1,
            Amount: 7500,
            Method: 1,
            Remark: "學生優惠",
            CreateTime: "2025-06-09T13:00:00.000",
            UpdateTime: "2025-06-09T13:00:00.000"
        }
    ];


    mock.onPost("/api/Order/AddOrderWithMembershipPlan").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })

    mock.onPost("/api/Order/AddOrderWithPersonalTrainingPackage").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })

    mock.onPost("/api/Order/AddOrder").reply(() => {
        const orderIdDto = {
            OrderId: 1
        }
        return [200, { ErrorCode: 1, ApiDataObject: orderIdDto }];
    })

    mock.onPost("/api/Order/PayByCard").reply(() => {
        return new Promise(resolve => {
            setTimeout(() => {
                const isSuccess = Math.random() > 0.5;
                resolve([200, { ErrorCode: isSuccess ? 10 : 1, ApiDataObject: { QrCodeString: "kpqwjej12312opkwqeopqkw12" } }]);
            }, 3000);
        });
    });

    mock.onPost("/api/Order/PayByCash").reply(() => {
        return [200, { ErrorCode: 1, ApiDataObject: { QrCodeString: "kpqwjej12312opkwqeopqkw12" } }];
    })

    mock.onPost("/api/Order/GetOrder").reply((config) => {
        let {
            State,
            Method,
            SortOrder,
            SortOption,
            RecordPerPage,
            Page
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = orders.filter(item => {
            const matchStatus = State === null || item.State === Number(State);
            const matchMethod = Method === null || item.Method === Number(Method);
            return matchStatus && matchMethod;
        });

        let field;
        // 2️⃣ 排序
        if (SortOption === "amount") {
            field = "Amount";
        } else if (SortOption === "state") {
            field = "State";
        }
        else {
            field = "OrderId"
        }

        filtered.sort((a, b) => {
            let aVal = a[field];
            let bVal = b[field];

            if (aVal < bVal) return SortOrder === 'descending' ? 1 : -1;
            if (aVal > bVal) return SortOrder === 'descending' ? -1 : 1;
            return 0;
        });

        // 3️⃣ 分頁
        const start = (Page - 1) * RecordPerPage;
        const paged = filtered.slice(start, start + RecordPerPage);

        const ApiDataObject = {
            OrderList: paged,
            TotalPage: Math.ceil(
                filtered.length / RecordPerPage
            )
        }

        return [200, { ErrorCode: 1, ApiDataObject }]
    })
}