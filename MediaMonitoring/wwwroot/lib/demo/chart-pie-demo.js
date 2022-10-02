// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

// Pie Chart Example
var ctx = document.getElementById("myPieChart");
var myPieChart = new Chart(ctx, {
  type: 'doughnut',
  data: {
    labels: ["MTN NIgeria", "Nestle Nigeria", "9 MObile", "Globacom NG","Multichoice NG"],
    datasets: [{
      data: [30, 20, 20,20,10],
      backgroundColor: ['#2FAB26', '#C60607', '#FF5252','#FFAB40','#E26111'],
      hoverBackgroundColor: ['#2FAB26', '#C60607', '#FF5252','#FFAB40','#E26111'],
      hoverBorderColor: "rgba(234, 236, 244, 1)",
    }],
  },
  options: {
    maintainAspectRatio: false,
    tooltips: {
      backgroundColor: "rgb(255,255,255)",
      bodyFontColor: "#858796",
      borderColor: '#dddfeb',
      borderWidth: 1,
      xPadding: 0,
      yPadding: 0,
      displayColors: false,
      caretPadding: 0,
    },
    legend: {
      display: false
    },
    cutoutPercentage: 40,
  },
});
