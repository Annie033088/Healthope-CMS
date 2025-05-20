export const groupClassIcon = [
    { value: '1', text: '❓' },
    { value: '2', text: '🏃‍♂️' },
    { value: '3', text: '❤️' },
    { value: '4', text: '⏱️' },
    { value: '5', text: '💪' },
    { value: '6', text: '🏋️' },
    { value: '7', text: '🏋️‍♂️' },
    { value: '8', text: '🧘‍♀️' },
    { value: '9', text: '🧘‍♂️' },
    { value: '10', text: '💃' },
    { value: '11', text: '🎶' },
    { value: '12', text: '🪩' },
    { value: '13', text: '👟' },
    { value: '14', text: '🚴‍♀️' },
    { value: '15', text: '🚲' },
    { value: '16', text: '💨' },
    { value: '17', text: '⭐' },
    { value: '18', text: '🪜' },
    { value: '19', text: '🎯' },
    { value: '20', text: '🔄' },
    { value: '21', text: '🎲' },
]

export default function valueToIcon(value) {
    groupClassIcon.forEach(icon => {
        if (icon.value === value.toString()) return icon.text;
    });
}

export const groupClassCategory = [
    { value: 'Cardio', text: '有氧' },
    { value: 'Force', text: '肌力' },
    { value: 'Yoga', text: '瑜伽' },
    { value: 'Dance', text: '舞蹈' },
    { value: 'Flywheel', text: '飛輪' },
    { value: 'Basic', text: '基礎' },
    { value: 'Other', text: '其他' },
]