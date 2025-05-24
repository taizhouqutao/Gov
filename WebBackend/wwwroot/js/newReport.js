$(function () {
  var hid_newTypeId = $("#hid_newTypeId").val();
  ajaxPie(1, "echarts-pie-chart1", '游览内容', '最近三十天点击量', hid_newTypeId);
  ajaxPie(2, "echarts-pie-chart2", '关注内容', '最近三十天咨询量', hid_newTypeId);
  ajaxLine(1, "echarts-line-chart1", '七日访问量', '点击量', hid_newTypeId);
  ajaxLine(2, "echarts-line-chart2", '七日留言数', '留言数', hid_newTypeId);
});

function ajaxLine(viewType, domid, text, subtext, hid_newTypeId) {
  $.ajax({
    url: "/Home/GetViewLogLines",
    contentType: "application/json",
    type: "POST", // 请求类型，通常是 GET 或 POST
    data: JSON.stringify({
      viewType: viewType,
      newTypeId: hid_newTypeId
    }),
    success: function (result) {
      if (result.ifSuccess) {
        $('#modal-form').modal('hide'); // 显示模态框，并触发 show 事件，从而设置内容。
        showlinechar(result.data, domid, text, subtext);
      }
      else {
        swal({
          title: "出错了",
          text: result.msg
        });
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      console.log('Status:', status);
      console.log('Details:', xhr.responseText);
      if (xhr.status == 401) {
        window.top.location.href = '/Home/login?returnUrl=' + encodeURIComponent(window.location.pathname);
      }
    }
  });
}

function showlinechar(data, domid, text, subtext) {
  var lineChart = echarts.init(document.getElementById(domid));
  var lineoption = {
    title: {
      text: text
    },
    tooltip: {
      trigger: 'axis'
    },
    legend: {
      data: [subtext]
    },
    grid: {
      x: 40,
      x2: 40,
      y2: 24
    },
    calculable: true,
    xAxis: [
      {
        type: 'category',
        boundaryGap: false,
        data: data.dayOfWeek
      }
    ],
    yAxis: [
      {
        type: 'value',
        axisLabel: {
          formatter: '{value} 次'
        }
      }
    ],
    series: [
      {
        name: subtext,
        type: 'line',
        data: data.viewCount,
        markPoint: {
          data: [
            { type: 'max', name: '最大值' },
            { type: 'min', name: '最小值' }
          ]
        },
        markLine: {
          data: [
            { type: 'average', name: '平均值' }
          ]
        }
      }
    ]
  };
  lineChart.setOption(lineoption);
  $(window).resize(lineChart.resize);
}

function ajaxPie(viewType, domid, text, subtext, hid_newTypeId) {
  $.ajax({
    url: "/Home/GetViewLogDetailReports",
    contentType: "application/json",
    type: "POST", // 请求类型，通常是 GET 或 POST
    data: JSON.stringify({
      viewType: viewType,
      newTypeId: hid_newTypeId
    }),
    success: function (result) {
      if (result.ifSuccess) {
        $('#modal-form').modal('hide'); // 显示模态框，并触发 show 事件，从而设置内容。
        showpiechar(result.data, domid, text, subtext);
      }
      else {
        swal({
          title: "出错了",
          text: result.msg
        });
      }
    },
    error: function (xhr, status, error) {
      console.error('Error:', error);
      console.log('Status:', status);
      console.log('Details:', xhr.responseText);
      if (xhr.status == 401) {
        window.top.location.href = '/Home/login?returnUrl=' + encodeURIComponent(window.location.pathname);
      }
    }
  });
}

function showpiechar(data, domid, text, subtext) {
  var pieChart = echarts.init(document.getElementById(domid));
  var pieoption = {
    title: {
      text: text,
      subtext: subtext,
      x: 'center'
    },
    tooltip: {
      trigger: 'item',
      formatter: "{a} <br/>{b} : {c} ({d}%)"
    },
    legend: {
      orient: 'vertical',
      x: 'left',
      data: data.newTypeName
    },
    calculable: true,
    series: [
      {
        name: '类型',
        type: 'pie',
        radius: '55%',
        center: ['50%', '60%'],
        data: data.details
      }
    ]
  };
  pieChart.setOption(pieoption);
  $(window).resize(pieChart.resize);
}
