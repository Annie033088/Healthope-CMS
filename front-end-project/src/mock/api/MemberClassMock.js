export default function (mock) {
    const memberPersonalTrainingPackage = [
        {
            "MemberPersonalTrainingPackageId": 2,
            "OrderId": 9,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 5,
            "PlanName": "12 堂基本課",
            "UsedSession": 5,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 07:24:11.301",
            "UpdateTime": "2025-06-16 07:24:11.301"
        },
        {
            "MemberPersonalTrainingPackageId": 3,
            "OrderId": 10,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 11,
            "PlanName": "12 堂基本課",
            "UsedSession": 5,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 07:25:54.136",
            "UpdateTime": "2025-06-16 07:25:54.136"
        },
        {
            "MemberPersonalTrainingPackageId": 4,
            "OrderId": 16,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 3,
            "PlanName": "12 堂基本課",
            "UsedSession": 8,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-16 08:25:48.539",
            "UpdateTime": "2025-06-16 08:25:48.539"
        },
        {
            "MemberPersonalTrainingPackageId": 5,
            "OrderId": 12,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 7,
            "PlanName": "12 堂基本課",
            "UsedSession": 0,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-17 08:11:14.658",
            "UpdateTime": "2025-06-17 08:11:14.658"
        },
        {
            "MemberPersonalTrainingPackageId": 6,
            "OrderId": 22,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 1,
            "PlanName": "12 堂基本課",
            "UsedSession": 1,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-20 01:50:05.010",
            "UpdateTime": "2025-06-20 01:50:05.010"
        },
        {
            "MemberPersonalTrainingPackageId": 7,
            "OrderId": 27,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 9,
            "PlanName": "12 堂基本課",
            "UsedSession": 3,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-23 05:47:03.224",
            "UpdateTime": "2025-06-23 05:47:03.224"
        },
        {
            "MemberPersonalTrainingPackageId": 8,
            "OrderId": 15,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 12,
            "PlanName": "12 堂基本課",
            "UsedSession": 3,
            "SessionCount": 12,
            "Status": 2,
            "CreateTime": "2025-06-24 09:00:25.063",
            "UpdateTime": "2025-06-24 09:00:25.063"
        },
        {
            "MemberPersonalTrainingPackageId": 9,
            "OrderId": 13,
            "CoachId": 8,
            "CoachPhone": 912345678,
            "CoachName": "Alice",
            "MemberId": 6,
            "PlanName": "12 堂基本課",
            "UsedSession": 6,
            "SessionCount": 12,
            "Status": 1,
            "CreateTime": "2025-06-24 09:08:36.987",
            "UpdateTime": "2025-06-24 09:08:36.987"
        }
    ]

    mock.onPost("/api/MemberClass/GetPersonalTrainingPackageAndCoach").reply(config => {
        let {
            MemberId,
        } = JSON.parse(config.data);

        // 1️⃣ 篩選
        let filtered = memberPersonalTrainingPackage.filter(item => {
            return MemberId === item.MemberId;
        });

        return [200, { ErrorCode: 1, ApiDataObject: filtered }]
    })

    mock.onPost("/api/MemberClass/AddMemberPersonalClass").reply(() => {
        return [200, { ErrorCode: 1}]
    })
}
